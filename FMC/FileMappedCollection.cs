using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Runtime.Versioning;

// ========================================================================================================================================
// This is a stand alone class which can be copied as is to other porojects because it does not have any other dependencies
// This class manages a binary file that can accept records of byte array and add them internally into the file. These records 
// can also be read or removed from the file. So this can be used as a general bucket of storing messages and then removing them. 
// You can think of this as:
// Concurrent Persisted List<byte[]>
// ========================================================================================================================================

/*
Metadata
~~~~~~~~
File-Header (Size is 16)
	FirstRecordStartAddress	Int32 (4 bytes) [value is -1 if no records]
	LastRecordStartAddress  Int32 (4 bytes) [value is -1 if no records]
	reserved for future		8 bytes

Record (Size is 16 + n)
	NextRecordStartAddress	Int32 (4 bytes) [value is -1 for first record]
	Data Size				Int32 (4 bytes) 
	reserved for future		8 bytes
	Data					n bytes
 
General Notes
~~~~~~~~~~~~~
. Archive file has the following properties: initial size, max size, and increment size. 
. File is binary, and can be extended if when adding a record not enought space is available and file size 
  did not exceed max size. 
. The class is thread safe cross process as we use Mutex. 
. File header is 16 bytes. Each record header is 16 bytes. 
. Data is stored and retreived as byte array (that's the only supported format)
. Messages can only be added after the last record (assuming there's enought space from there till the end of the file),
  or else a defrag should take place to free available space if exists between records, if also defrag 
  did not recover enough space, the file size will be incremented if it still did not reach its max size). 
. When adding the message and the file is already extended beyond the initial size, and if the add succeeds, 
  the following action takes place: if the last position with data on the file is smaller than the size 
  of the initial file size / 2 then a shrink will tae place to reduce the file size to the initial size. 
. Messages can be removed per index (thus creating 'holes' and fragmentation within the file)
. When shrinking the messages, space at the beginig and between the records removed and records are placed 
  one after the other. 
. Each record points to the next one after. Last record points to NULL. 
. Extend and Defrag are auto invoked internally per need. 
. Shrink can be are auto invoked internally per need (when EnableAutoShrink is set) and/or done by a client caller. 


File Structure
~~~~~~~~~~~~~~
Sample file with 3 records (not fragmented):
 1        2    3
Hh========h====h===========================---------------------------------------|

Sample file with 3 records after one middle record was removed:
 1             2
Hh========-----h===========================---------------------------------------|

Sample file with 3 fragmented records:
         1                2                       3
H--------h========--------h====-------------------h===========================----|

Sample with 3 records after defrag:
 1        2    3
Hh========h====h===========================---------------------------------------|

Sample with 3 records after extend:
 1        2    3
Hh========h====h===========================------------------------------------------------------------|
 
Sample with 3 records after shrink:
 1        2    3
Hh========h====h===========================---------------------------------------|
 
Legend:
   H  File header
   h  Record header
   -  Free space
   =  Record body data
   |  End of file

Main Methods
~~~~~~~~~~~~
Put()
TryGetAt()
TryRemoveAt()
TryPeekAt()
Count()
Peek()
PeekAddresses()
Defrag()
ShrinkFile()
ExtendFile()
TryGetImmediateAvailableSpace()
TryGetAvailableSpace()

*/


public class FileMappedCollection { // records collection file

	public string SystemWideIdentifier { get; private set; }
	public string FilePath { get; private set; }

	public readonly int FileSizeMaxBytes;
	public readonly int FileSizeIncrementBytes;
	public readonly int FileSizeInitialBytes;
	public bool EnableAutoShrink { get; private set; }
	public int FileSizeBytes { get; private set; }

	private const int NULL = -1;
	public const int FILE_HEADER_SIZE = 16; // all 16 bytes at the begning of the file are reserved for the file header
	public const int RECORD_HEADER_SIZE = 16;

	private readonly string MUTEX_NAME;
	private MutexSecurity mMutexSecuritySettings;
	Mutex mutex;

	public enum RecordRetreivalStatus { RecordOutOfRangeError, RecordNotFoundError, LockNotAquiredError, Success };
	public string InitializationFileCorruptionError { get; private set; }

	private static EventWaitHandle mRecordAddedEvent;

	#region utility
	private static ASCIIEncoding enc = new System.Text.ASCIIEncoding();
	public static byte[] StringToByteArray(string str) { return enc.GetBytes(str); }
	public static string ByteArrayToString(byte[] dBytes) { return enc.GetString(dBytes); }
	#endregion

	public FileMappedCollection(string filePath, int initialFileSizeBytes, int maxFileSizeBytes, int incrementSizeBytes, bool regenerateFileIffoundCorrupt, bool enableAutoShrink)
		: this(Path.GetFileNameWithoutExtension(filePath), filePath, initialFileSizeBytes, maxFileSizeBytes, incrementSizeBytes, regenerateFileIffoundCorrupt, enableAutoShrink) {
	}

	/// <summary>
	/// Main Constructor
	/// </summary>
	/// <param name="systemWideIdentifier"> Must not contain any slash / \ </param>
	/// <param name="filePath"></param>
	/// <param name="initialFileSizeBytes"></param>
	/// <param name="maxFileSizeBytes"></param>
	/// <param name="incrementSizeBytes"></param>
	/// <param name="regenerateFileIffoundCorrupt"></param>
	[SupportedOSPlatform("linux")]
	[SupportedOSPlatform("windows")]
	public FileMappedCollection(string systemWideIdentifier, string filePath, int initialFileSizeBytes, int maxFileSizeBytes, int incrementSizeBytes, bool regenerateFileIffoundCorrupt, bool enableAutoShrink) {
		FilePath = filePath;
		FileSizeInitialBytes = (File.Exists(FilePath) ? ((int)(new FileInfo(FilePath)).Length) : initialFileSizeBytes);
		FileSizeMaxBytes = maxFileSizeBytes;
		FileSizeIncrementBytes = incrementSizeBytes;
		SystemWideIdentifier = systemWideIdentifier;
		EnableAutoShrink = enableAutoShrink;

		if (FileSizeInitialBytes <= 1024) throw new Exception(string.Format("AMS Notifier Thread Initialization Error: Invalid argument for initialFileSizeBytes {0}, value must be greater than 1024. Consider changing host properties, pushing new psdb.xml, and restarting the AMS service. ", FileSizeInitialBytes));
		if (maxFileSizeBytes <= 1024) throw new Exception(string.Format("AMS Notifier Thread Initialization Error: Invalid argument for maxFileSizeBytes {0}, value must be greater than 1024. Consider changing host properties, pushing new psdb.xml, and restarting the AMS service. ", maxFileSizeBytes));
		if (incrementSizeBytes < 0) throw new Exception(string.Format("AMS Notifier Thread Initialization Error: Invalid argument for incrementSizeBytes {0}, value must be greater than 1024. Consider changing host properties, pushing new psdb.xml, and restarting the AMS service. ", incrementSizeBytes));
		if (FileSizeInitialBytes > FileSizeMaxBytes) throw new Exception(string.Format("AMS Notifier Thread Initialization Error: Initial file size {0} cannot exceed max file size {1} bytes. Consider changing host properties, pushing new psdb.xml, and restarting the AMS service OR resetting the notifier file {2}", FileSizeInitialBytes, maxFileSizeBytes, filePath));
		if (incrementSizeBytes > FileSizeMaxBytes - FileSizeInitialBytes) throw new Exception(string.Format("AMS Notifier Thread Initialization Error: Increment file size {0} cannot exceed max minus initial file size {1} bytes. Consider changing host properties, pushing new psdb.xml, and restarting the AMS service OR resetting the notifier file {2}",
			incrementSizeBytes, maxFileSizeBytes - FileSizeInitialBytes, filePath));

		// Mutex setup
		MUTEX_NAME = string.Format("Global\\Intel:{0}", SystemWideIdentifier);
		bool createdNew = false; ;
		MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
		mMutexSecuritySettings = new MutexSecurity();
		mMutexSecuritySettings.AddAccessRule(allowEveryoneRule);
		mutex = new Mutex(false, MUTEX_NAME, out createdNew);
		mutex.SetAccessControl(mMutexSecuritySettings);

		// setup the system wide (cross process) event wait handle security settings
		bool created_new = false;
		//EventWaitHandleAccessRule allowEveryoneEventRule = new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
		//  EventWaitHandleRights.Synchronize | EventWaitHandleRights.Modify | EventWaitHandleRights.ReadPermissions, AccessControlType.Allow);
		EventWaitHandleAccessRule allowEveryoneEventRule = new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
			EventWaitHandleRights.FullControl, AccessControlType.Allow);
		EventWaitHandleSecurity eventSecurity = new EventWaitHandleSecurity();
		eventSecurity.AddAccessRule(allowEveryoneEventRule);
		// setup the system wide (cross process) event wait handle
		mRecordAddedEvent = new EventWaitHandle(true, EventResetMode.AutoReset, SystemWideIdentifier, out created_new);
		mRecordAddedEvent.SetAccessControl(eventSecurity);

		// in case asked to rebuild the file when corrupted
		bool already_verified_consistency = false;
		if (regenerateFileIffoundCorrupt) {
			string error_msg;
			if (File.Exists(filePath)) {
				if (!VerifyConsistency(out error_msg)) {
					if (AcquireMutex()) {
						try {
							File.Delete(filePath);
						} finally {
							ReleaseMutex();
						}
					} else {
						throw new Exception(string.Format("Mutex was not aquired upon RCF {0} startup", filePath));
					}
				} else {
					already_verified_consistency = true;
				}
			}
		}

		// Get started with the RCF File
		if (!File.Exists(FilePath)) {
			// create the file
			if (AcquireMutex()) {
				try {
					using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None)) {
						fs.SetLength(FileSizeInitialBytes);
					}
				} finally {
					ReleaseMutex();
				}
			} else {
				throw new Exception(string.Format("Mutex was not aquired upon RCF {0} startup", filePath));
			}
			// write the new file header
			if (!WriteHeader(NULL, NULL)) throw new Exception(string.Format("Error initializing file header on \"{0}\"", filePath));
		} else {
			// file exists so just verify its consistency
			if (!already_verified_consistency) {
				string error_msg;
				if (!VerifyConsistency(out error_msg)) {
					throw new Exception(string.Format("File \"{0}\" was found inconsistent. {1}", filePath, error_msg));
				}
			}
		}
		FileSizeBytes = (int)(new FileInfo(filePath)).Length;
	}

	#region Mutex related
	private int mAbandonedMutexExceptionCount = 0;
	public int AbandonedMutexExceptionCount { get { return mAbandonedMutexExceptionCount; } }
	public void ResetAbandonedMutexExceptionCount() { Interlocked.Exchange(ref mAbandonedMutexExceptionCount, 0); }

	// MutexLock()
	private bool AcquireMutex() {
		// if it was not acquired, it timed out
		try {
			//if (!mutex.WaitOne(8000)) {
			//	throw new Exception(string.Format("Fatal Error. Mutex not acquired after 5 sec for {0}", MUTEX_NAME));
			//}
			return mutex.WaitOne(8000);
		} catch (AbandonedMutexException ex) {
			// abandoned mutexes are still acquired, we just need
			// to handle the exception and treat it as acquisition
			// it means another process aquired the mutex but them crashed and the OS has released the mutex
			Interlocked.Increment(ref mAbandonedMutexExceptionCount);
			//string err_msg;
			//if (!VerifyConsistency(out err_msg)) throw new Exception(string.Format("AcquireMutex: File corruption post {0} AbandonedMutexException raised. Source: {1}  Application: {2}. {3}",
			//	mAbandonedMutexExceptionCount, ex.TargetSite.Name ?? "Null", ex.Source ?? "Null", err_msg), ex);
			return true;
		}
	}

	private bool AcquireMutexLong() {
		// if it was not acquired, it timed out
		try {
			//if (!mutex.WaitOne(8000)) {
			//	throw new Exception(string.Format("Fatal Error. Mutex not acquired after 5 sec for {0}", MUTEX_NAME));
			//}
			return mutex.WaitOne(16000);
		} catch (AbandonedMutexException ex) {
			// abandoned mutexes are still acquired, we just need
			// to handle the exception and treat it as acquisition
			// it means another process aquired the mutex but them crashed and the OS has released the mutex
			Interlocked.Increment(ref mAbandonedMutexExceptionCount);
			//string err_msg;
			//if (!VerifyConsistency(out err_msg)) throw new Exception(string.Format("AcquireMutexLong: File corruption post {0} AbandonedMutexException raised. Source: {1}  Application: {2}. {3}",
			//	mAbandonedMutexExceptionCount, ex.TargetSite.Name ?? "Null", ex.Source ?? "Null", err_msg), ex);
			return true;
		}
	}

	//MutexRelease()
	private void ReleaseMutex() {
		mutex.ReleaseMutex();
	}
	#endregion

	#region Event Handles
	public void SetRecordsAddedEvent() {
		mRecordAddedEvent.Set();
	}
	private void ResetRecordsAddedEvent() {
		mRecordAddedEvent.Reset();
	}
	public bool WaitForRecords(int maxMilliSecondsToWait) {
		return mRecordAddedEvent.WaitOne(maxMilliSecondsToWait);
	}
	//public void WaitForRecords(int maxMilliSecondsToWait, ManualResetEventSlim extraStopWaitEvent) {
	//	//WaitHandle.WaitAny(new WaitHandle[] { mRecordAddedEvent, extraStopWaitEvent.WaitHandle }, maxMilliSecondsToWait);
	//	const int INTERVAL = 400;
	//	for (int i = 0; i < maxMilliSecondsToWait / INTERVAL; i++) {
	//		if (mRecordAddedEvent.WaitOne(INTERVAL) || extraStopWaitEvent.IsSet) return;			
	//	}		
	//}
	#endregion

	#region sub classes
	public struct RecordAddress {
		public int StartAddress;
		public int NextRecordStartAddress;
		public int RecordBodyLength;
		public RecordAddress(int startAddress, int nextRecordStartAddress, int recordBodyLength) {
			StartAddress = startAddress;
			NextRecordStartAddress = nextRecordStartAddress;
			RecordBodyLength = recordBodyLength;
		}
		public int RecordSize { get { return RecordBodyLength + RECORD_HEADER_SIZE; } }
	}

	public class Record {
		public readonly int StartAddress;
		public readonly int NextRecordStartAddress;
		public readonly int RecordBodyLength;
		public readonly byte[] RecordBody;
		public Record(int startAddress, int nextRecordStartAddress, int recordBodyLength, byte[] recordBody = null) {
			StartAddress = startAddress;
			NextRecordStartAddress = nextRecordStartAddress;
			RecordBodyLength = recordBodyLength;
			RecordBody = recordBody;
		}
		public int RecordSize { get { return RecordBodyLength + RECORD_HEADER_SIZE; } }
	}
	#endregion

	public bool ApproximatePercentUtil(out int percentUtil) {
		int firstRecordStartAddress;
		int lastRecordStartAddress;
		// this is an approximation because we ignore the size of the last record

		percentUtil = 9;
		if (ReadHeader(out firstRecordStartAddress, out lastRecordStartAddress)) {
			if (firstRecordStartAddress >= 0 && lastRecordStartAddress >= 0) {
				// assuming last record has 22 bytes (guess)
				percentUtil = 100 - (int)Math.Floor(
					(FileSizeBytes - (lastRecordStartAddress - firstRecordStartAddress + 22))
					   /
					((float)FileSizeBytes / 100)
					);
			} else {
				percentUtil = 0;
			}
			return true;
		} else {
			return false;
		}
	}

	public bool ReadHeader(out int firstRecordStartAddress, out int lastRecordStartAddress) {
		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						firstRecordStartAddress = r.ReadInt32();
						lastRecordStartAddress = r.ReadInt32();
					}
				}
				return true;
			} finally {
				ReleaseMutex();
			}
		} else {
			firstRecordStartAddress = -1;
			lastRecordStartAddress = -1;
			return false;
		}
	}

	// private method that should only be called by methods who already aquired mutex
	// note that the values returned by this may be outside the file size if last record is close to the end of the file
	private void NewRecordPotential(out int newRecordStartAddress, out int currentLastRecordStartAddress) {
		// newRecordStartAddress would become the last record eventually if all goes well
		int first_record_start, last_record_next_address, record_data_size;
		using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
			using (BinaryReader r = new BinaryReader(fs)) {
				// read the file header
				first_record_start = r.ReadInt32();
				currentLastRecordStartAddress = r.ReadInt32();

				if (currentLastRecordStartAddress != NULL) {
					// read the last record header
					r.BaseStream.Seek(currentLastRecordStartAddress, SeekOrigin.Begin);
					last_record_next_address = r.ReadInt32(); // should be NULL
					record_data_size = r.ReadInt32();

					// verify file header and record headers have values in the expected range
					if (last_record_next_address != NULL) throw new Exception("Expected to get NULL on next record address for the last record.");

					// this means new record will be outside of file size but we dont need to throw exception, simply return the (unreasonable) location, caller should make the decisions accordingly
					// if location if after file size. 
					//if (currentLastRecordStartAddress >= FileSizeBytes || currentLastRecordStartAddress < 16) throw new Exception("Last record start address is out of file range.");

					// so here's where a new record would be placed after the last existing record
					newRecordStartAddress = currentLastRecordStartAddress + RECORD_HEADER_SIZE + record_data_size;
				} else {
					newRecordStartAddress = FILE_HEADER_SIZE;
				}
			}
		}
	}

	// private method that should only be called by methods who already aquired mutex
	private bool CanPutNewRecord(int newRecordBodySize, out int newRecordStartAddress, out int currentLastRecordStartAddress) {
		// newRecordStartAddress would become the last record eventually if all goes well
		NewRecordPotential(out newRecordStartAddress, out currentLastRecordStartAddress);
		return FileSizeBytes - newRecordStartAddress > newRecordBodySize + RECORD_HEADER_SIZE;
	}

	// private method that should only be called by methods who already aquired mutex
	private int NextRecordWriteAddress {
		get {
			int newRecordStartAddress; int currentLastRecordStartAddress;
			NewRecordPotential(out newRecordStartAddress, out currentLastRecordStartAddress);
			return newRecordStartAddress;
		}
	}

	// this should be the only place where header is updated
	// private method that should only be called by methods who already aquired mutex
	private bool WriteHeader(int firstRecordStartAddress, int lastRecordStartAddress) {
		try {
			byte[] h = new byte[8];
			Array.Copy(BitConverter.GetBytes(firstRecordStartAddress), 0, h, 0, 4);
			Array.Copy(BitConverter.GetBytes(lastRecordStartAddress), 0, h, 4, 4);

			// do it in one write trasation (for 64 bit machines, doing at 8 bytes[] operation is atomic)
			using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Write, FileShare.None)) {
				using (BinaryWriter w = new BinaryWriter(fs)) {
					w.Write(h);
					w.Flush();
				}
			}
			return true;
		} catch {
			return false;
		}
	}

	public bool TryHasRecords(out bool hasRecords) {
		int firstRecordStartAddress;
		int lastRecordStartAddress;
		if (ReadHeader(out firstRecordStartAddress, out lastRecordStartAddress)) {
			hasRecords = (firstRecordStartAddress != NULL);
			return true;
		} else {
			hasRecords = false;
			return false;
		}
	}

	public bool TryGetImmediateAvailableSpace(out int availableBytes) {
		int firstRecordStartAddress;
		int lastRecordStartAddress;
		if (ReadHeader(out firstRecordStartAddress, out lastRecordStartAddress)) {
			availableBytes = FileSizeBytes - lastRecordStartAddress; //need also to substract the last record length, but we're ignoring it assuming it is small comared to over all file
			return true;
		} else {
			availableBytes = -1;
			return false;
		}
	}

	public bool TryEstimateFreeSpace(out int availableBytes) {
		int firstRecordStartAddress;
		int lastRecordStartAddress;
		if (ReadHeader(out firstRecordStartAddress, out lastRecordStartAddress)) {
			availableBytes = FileSizeBytes - (lastRecordStartAddress - firstRecordStartAddress);
			return true;
		} else {
			availableBytes = -1;
			return false;
		}
	}

	public bool TryGetAvailableSpace(out int availableBytes) {
		int firstRecordStartAddress;
		int lastRecordStartAddress;
		List<RecordAddress> l;
		int freeBytes;

		if (ReadHeader(out firstRecordStartAddress, out lastRecordStartAddress) && TryPeekAddresses(out l, out freeBytes)) {
			if (l.Count == 0) {
				availableBytes = FileSizeBytes - FILE_HEADER_SIZE;
			} else {
				availableBytes = freeBytes + (FileSizeBytes - lastRecordStartAddress - l[l.Count - 1].RecordSize);
			}
			return true;
		} else {
			availableBytes = -1;
			return false;
		}
	}


	// private method that should only be called by methods who already aquired mutex
	private void PutNewRecord(int lastRecordStartAddress, int newRecordStartAddress, byte[] data) {
		// Create the new record header
		byte[] rh = new byte[8];
		Array.Copy(BitConverter.GetBytes(NULL), 0, rh, 0, 4);            // Header Record: Next Pointer. // since this would become the last record
		Array.Copy(BitConverter.GetBytes(data.Length), 0, rh, 4, 4);     // Header Record: Body Length 

		using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) {
			using (BinaryWriter w = new BinaryWriter(fs)) {
				// write the new record
				w.BaseStream.Seek(newRecordStartAddress, SeekOrigin.Begin);
				w.Write(rh);
				w.BaseStream.Seek(8, SeekOrigin.Current);
				w.Write(data);

				// update the pointer to the next record in what used to be the last record
				if (lastRecordStartAddress == NULL) {
					// in case we put the first record then just update the header
					w.BaseStream.Seek(0, SeekOrigin.Begin);
					w.Write(newRecordStartAddress);
				} else {
					w.BaseStream.Seek(lastRecordStartAddress, SeekOrigin.Begin);
					w.Write(newRecordStartAddress);
				}

				// update the header that this is now th elast record
				w.BaseStream.Seek(4, SeekOrigin.Begin);
				w.Write(newRecordStartAddress);

				// Flush
				w.Flush();
			}
		}
	}

	public bool TryPut(byte[] bytes) {
		// always adds the record as the last one !!

		int last_record_start; // what is now the last record in th DB
		int new_record_start; // where we would put the new record (and after that it will become the last record in the file)

		if (AcquireMutex()) {
			try {
				if (CanPutNewRecord(bytes.Length, out new_record_start, out last_record_start)) {
					PutNewRecord(last_record_start, new_record_start, bytes);
					if (EnableAutoShrink) {
						if (new_record_start + bytes.Length + RECORD_HEADER_SIZE < FileSizeInitialBytes / 2) DecreaseFileSizeToinitialSize();
					}
					return true;
				} else {
					// lets shrink the data and check again
					if (Defrag() > bytes.Length + RECORD_HEADER_SIZE) {
						if (CanPutNewRecord(bytes.Length, out new_record_start, out last_record_start)) {
							PutNewRecord(last_record_start, new_record_start, bytes);
							return true;
						} else {
							return false;
						}
					} else {
						// lets try to increase the file size
						if (IncreaseFileSize()) {
							if (CanPutNewRecord(bytes.Length, out new_record_start, out last_record_start)) {
								PutNewRecord(last_record_start, new_record_start, bytes);
								return true;
							} else {
								return false;
							}
						} else {
							return false;
						}
					}
				}
			} finally {
				ReleaseMutex();
			}
		} else {
			return false;
		}

	}

	public RecordRetreivalStatus TryRemoveAt(int recordStartAddress) {
		int prev_record_start, this_record_start, next_record_start, record_data_size;
		int header_first_record_start, header_last_record_start;
		int new_header_first_record_start, new_header_last_record_start;
		bool is_last_index = false;
		bool is_first_index = false;

		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						prev_record_start = NULL;
						r.BaseStream.Seek(0, SeekOrigin.Begin);
						header_first_record_start = r.ReadInt32();
						header_last_record_start = r.ReadInt32();

						// in case the file is empty or the address requested is not within range per header file
						if (header_first_record_start == NULL || recordStartAddress < header_first_record_start || recordStartAddress > header_last_record_start) {
							return RecordRetreivalStatus.RecordOutOfRangeError;
						}

						// first record address read from the header
						this_record_start = header_first_record_start;

						r.BaseStream.Seek(this_record_start, SeekOrigin.Begin);
						next_record_start = r.ReadInt32();
						record_data_size = r.ReadInt32();

						// walk through the records until found the seeked address
						while (recordStartAddress > this_record_start && next_record_start != NULL) {
							prev_record_start = this_record_start;
							r.BaseStream.Seek(next_record_start, SeekOrigin.Begin);
							this_record_start = next_record_start;
							next_record_start = r.ReadInt32();
							record_data_size = r.ReadInt32();
						}

						if (recordStartAddress == this_record_start) {
							r.BaseStream.Seek(8, SeekOrigin.Current);
						} else {
							return RecordRetreivalStatus.RecordNotFoundError;
						}

						if (next_record_start == NULL) is_last_index = true;
						if (recordStartAddress == header_first_record_start) is_first_index = true;

					}
				}
				// deal with the new header
				new_header_first_record_start = header_first_record_start;
				new_header_last_record_start = header_last_record_start;
				if (is_first_index) {
					new_header_first_record_start = next_record_start;
				}
				if (is_last_index) { // if removed the last record
					new_header_last_record_start = prev_record_start;
				}
				if (is_first_index || is_last_index) { // need to update the header only if removed the last or the first record
					WriteHeader(new_header_first_record_start, new_header_last_record_start);
				}

				// reset the event only if we removed the last and only record from the file
				if (is_first_index && is_last_index) ResetRecordsAddedEvent();

				// update the records pointers only if this is not the first record otherwise all the pointers are already set in the records becauise they only consist of next record pointers
				if (!is_first_index) {
					using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Write, FileShare.None)) {
						using (BinaryWriter w = new BinaryWriter(fs)) {
							// update the next record that it becomes the first one one
							w.BaseStream.Seek(prev_record_start, SeekOrigin.Begin);
							w.Write(next_record_start);
							w.Flush();
						}
					}
				}
				// return the value
				return RecordRetreivalStatus.Success;
			} finally {
				ReleaseMutex();
			}
		} else {
			return RecordRetreivalStatus.LockNotAquiredError;
		}
	}

	public delegate bool ProcessRecord(byte[] record);

	public bool PeekAndRemoveIfWellProcessed(ProcessRecord processRecordHandler, int recordIndex = 0) {
		int prev_record_start, this_record_start, next_record_start, record_data_size;
		int header_first_record_start, header_last_record_start;
		int new_header_first_record_start, new_header_last_record_start;
		byte[] b = null;
		bool is_last_index = false;

		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						prev_record_start = NULL;
						r.BaseStream.Seek(0, SeekOrigin.Begin);
						header_first_record_start = r.ReadInt32();
						header_last_record_start = r.ReadInt32();

						this_record_start = header_first_record_start; // first record address read from the header
						if (this_record_start == NULL) {
							// file is empty of records
							return false;
						}

						r.BaseStream.Seek(this_record_start, SeekOrigin.Begin);
						next_record_start = r.ReadInt32();
						record_data_size = r.ReadInt32();

						// walk through the following records
						for (int i = 0; i < recordIndex; i++) {
							// read the next
							if (next_record_start != NULL) {
								prev_record_start = this_record_start;
								r.BaseStream.Seek(next_record_start, SeekOrigin.Begin);
								this_record_start = next_record_start;
								next_record_start = r.ReadInt32();
								record_data_size = r.ReadInt32();
							} else {
								return false;
							}
						}

						r.BaseStream.Seek(8, SeekOrigin.Current);
						b = r.ReadBytes(record_data_size);
						if (next_record_start == NULL) is_last_index = true;
					}
				}

				if (processRecordHandler(b)) {
					// update pointers to remove this record
					// deal with the new header
					new_header_first_record_start = header_first_record_start;
					new_header_last_record_start = header_last_record_start;
					if (recordIndex == 0) {
						new_header_first_record_start = next_record_start;
					}
					if (is_last_index) { // if removed the last record
						new_header_last_record_start = prev_record_start;
					}
					if (recordIndex == 0 || is_last_index) { // need to update the header only if removed the last or the first record
						WriteHeader(new_header_first_record_start, new_header_last_record_start);
					}

					// update the records pointers only if this is not the first record otherwise all the pointers are already set in the records becauise they only consist of next record pointers
					if (recordIndex > 0) {
						using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Write, FileShare.None)) {
							using (BinaryWriter w = new BinaryWriter(fs)) {
								// update the next record that it becomes the first one one
								w.BaseStream.Seek(prev_record_start, SeekOrigin.Begin);
								w.Write(next_record_start);
								w.Flush();
							}
						}
					}
					// return the value
					return true;
				} else {
					return false;
				}
			} finally {
				ReleaseMutex();
			}
		} else {
			return false;
		}

	}

	// Removes a record and returns its value. Returns null if no such record exists. 
	// record index by default is Zero meaning getting the first (oldest) record entered
	public RecordRetreivalStatus TryGetAt(int recordStartAddress, out byte[] recordBytes) {
		int prev_record_start, this_record_start, next_record_start, record_data_size;
		int header_first_record_start, header_last_record_start;
		int new_header_first_record_start, new_header_last_record_start;
		bool is_last_index = false;
		bool is_first_index = false;

		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						prev_record_start = NULL;
						r.BaseStream.Seek(0, SeekOrigin.Begin);
						header_first_record_start = r.ReadInt32();
						header_last_record_start = r.ReadInt32();

						// in case the file is empty or the address requested is not within range per header file
						if (header_first_record_start == NULL || recordStartAddress < header_first_record_start || recordStartAddress > header_last_record_start) {
							recordBytes = null;
							return RecordRetreivalStatus.RecordNotFoundError;
						}

						// first record address read from the header
						this_record_start = header_first_record_start;

						// goto the first record
						r.BaseStream.Seek(this_record_start, SeekOrigin.Begin);
						next_record_start = r.ReadInt32();
						record_data_size = r.ReadInt32();

						// walk through the following records
						while (recordStartAddress > this_record_start && next_record_start != NULL) {
							prev_record_start = this_record_start;
							r.BaseStream.Seek(next_record_start, SeekOrigin.Begin);
							this_record_start = next_record_start;
							next_record_start = r.ReadInt32();
							record_data_size = r.ReadInt32();
						}

						if (recordStartAddress == this_record_start) {
							r.BaseStream.Seek(8, SeekOrigin.Current); // record header has extra 8 bytes (for future use) so skipping it
							recordBytes = r.ReadBytes(record_data_size);
						} else {
							recordBytes = null;
							return RecordRetreivalStatus.RecordNotFoundError;
						}

						if (next_record_start == NULL) is_last_index = true;
						if (recordStartAddress == header_first_record_start) is_first_index = true;

					}
				}
				// update pointers to remove this record
				// deal with the new header
				new_header_first_record_start = header_first_record_start;
				new_header_last_record_start = header_last_record_start;
				if (is_first_index) {
					new_header_first_record_start = next_record_start;
				}
				if (is_last_index) { // if removed the last record
					new_header_last_record_start = prev_record_start;
				}
				if (is_first_index || is_last_index) { // need to update the header only if removed the last or the first record
					WriteHeader(new_header_first_record_start, new_header_last_record_start);
				}

				// update the records pointers only if this is not the first record otherwise all the pointers are already set in the records becauise they only consist of next record pointers
				if (!is_first_index) {
					using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Write, FileShare.None)) {
						using (BinaryWriter w = new BinaryWriter(fs)) {
							// update the next record that it becomes the first one 
							w.BaseStream.Seek(prev_record_start, SeekOrigin.Begin);
							w.Write(next_record_start);
							w.Flush();
						}
					}
				}
				// return the value
				return (recordBytes == null ? RecordRetreivalStatus.RecordNotFoundError : RecordRetreivalStatus.Success);
			} finally {
				ReleaseMutex();
			}
		} else {
			recordBytes = null;
			return RecordRetreivalStatus.LockNotAquiredError;
		}
	}

	// returns null if no such record
	public RecordRetreivalStatus TryPeekAt(int recordStartAddress, out byte[] recordBytes) {
		int prev_record_start, this_record_start, next_record_start, record_data_size;
		int header_first_record_start, header_last_record_start;

		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						prev_record_start = NULL;
						r.BaseStream.Seek(0, SeekOrigin.Begin);
						header_first_record_start = r.ReadInt32();
						header_last_record_start = r.ReadInt32();

						// in case the file is empty or the address requested is not within range per header file
						if (header_first_record_start == NULL || recordStartAddress < header_first_record_start || recordStartAddress > header_last_record_start) {
							recordBytes = null;
							return RecordRetreivalStatus.RecordNotFoundError;
						}

						// first record address read from the header
						this_record_start = header_first_record_start;

						// move to the 1st record
						r.BaseStream.Seek(this_record_start, SeekOrigin.Begin);
						next_record_start = r.ReadInt32();
						record_data_size = r.ReadInt32();

						// walk through the records until found the seeked address
						while (recordStartAddress > this_record_start && next_record_start != NULL) {
							prev_record_start = this_record_start;
							r.BaseStream.Seek(next_record_start, SeekOrigin.Begin);
							this_record_start = next_record_start;
							next_record_start = r.ReadInt32();
							record_data_size = r.ReadInt32();
						}

						if (recordStartAddress == this_record_start) {
							r.BaseStream.Seek(8, SeekOrigin.Current);
							recordBytes = r.ReadBytes(record_data_size);
						} else {
							recordBytes = null;
						}
					}
				}
				// return the value
				return (recordBytes == null ? RecordRetreivalStatus.RecordNotFoundError : RecordRetreivalStatus.Success);
			} finally {
				ReleaseMutex();
			}
		} else {
			recordBytes = null;
			return RecordRetreivalStatus.LockNotAquiredError;
		}
	}

	public bool TryCount(out int numRecords) {
		int first_record_start, next_record_start;
		numRecords = 0;

		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						// read the file header
						first_record_start = r.ReadInt32();

						// walk through all record			
						next_record_start = first_record_start;
						while (next_record_start != NULL) {
							// read the next record header
							r.BaseStream.Seek(next_record_start, SeekOrigin.Begin);
							next_record_start = r.ReadInt32();
							numRecords += 1;
						}
					}
				}
				return true;
			} finally {
				ReleaseMutex();
			}
		} else {
			numRecords = -1;
			return false;
		}
	}

	public bool TryClear() {
		if (AcquireMutex()) {
			try {
				return WriteHeader(NULL, NULL);
			} finally {
				ResetRecordsAddedEvent();
				ReleaseMutex();
			}
		} else {
			return false;
		}
	}

	public bool ExtendFile() {
		if (AcquireMutex()) {
			try {
				return IncreaseFileSize();
			} finally {
				ReleaseMutex();
			}
		} else {
			return false;
		}
	}

	public bool ShrinkFile() {
		if (AcquireMutex()) {
			try {
				return DecreaseFileSizeToinitialSize();
			} finally {
				ReleaseMutex();
			}
		} else {
			return false;
		}
	}

	public bool Defrag(out int releasedBytes) {
		if (AcquireMutex()) {
			try {
				releasedBytes = Defrag();
				return true;
			} finally {
				ReleaseMutex();
			}
		} else {
			releasedBytes = -1;
			return false;
		}
	}

	public bool VerifyConsistency(out string errorMessage) {
		if (AcquireMutex()) {
			try {
				int first_record_start, last_record_start, next_record_start, record_data_size;
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						// read the file header
						first_record_start = r.ReadInt32();
						last_record_start = r.ReadInt32();

						if (last_record_start == NULL && last_record_start == NULL) {
							// were OK file is empty with no record
						} else if (last_record_start == NULL && last_record_start != NULL) {
							errorMessage = "File header corruption, either both first/last records point to NULL, or both should point to valid addresses";
							return false;
						} else if (last_record_start != NULL && last_record_start == NULL) {
							errorMessage = "File header corruption, either both first/last records point to NULL, or both should point to valid addresses";
							return false;
						} else if (last_record_start < 16 || last_record_start < 16) {
							errorMessage = "File header corruption, invalid address location indicated for first or last records pointer.";
							return false;
						} else {
							// read the last record header
							r.BaseStream.Seek(last_record_start, SeekOrigin.Begin);
							next_record_start = r.ReadInt32(); // should be NULL
							record_data_size = r.ReadInt32();
							if (next_record_start != NULL) { errorMessage = "Last record header should have NULL on next pointer"; return false; }
							if (record_data_size < 0) { errorMessage = "Data size cannot be less than zero."; return false; }

							// read the first record header
							r.BaseStream.Seek(first_record_start, SeekOrigin.Begin);
							next_record_start = r.ReadInt32();
							record_data_size = r.ReadInt32();
							if (record_data_size < 0) { errorMessage = "Data size cannot be less than zero."; return false; }

							// walk through all record
							int this_record_start_address;
							while (next_record_start != NULL) {
								// read the next record header
								this_record_start_address = next_record_start;
								r.BaseStream.Seek(this_record_start_address, SeekOrigin.Begin);
								next_record_start = r.ReadInt32();
								record_data_size = r.ReadInt32();
								if (record_data_size < 0) { errorMessage = "Data size cannot be less than zero."; return false; }
								if (next_record_start != NULL) {
									if (record_data_size > (next_record_start - this_record_start_address - RECORD_HEADER_SIZE)) { errorMessage = "Data size cannot be less than zero."; return false; }
								}
							}
						}
					}
				}
				// looks like we're OK
				errorMessage = "";
				return true;
			} catch (Exception ex) {
				errorMessage = ex.Message;
				return false;
			} finally {
				ReleaseMutex();
			}
		} else {
			errorMessage = string.Format("Mutex was not aquired on RCF {0} ", MUTEX_NAME);
			return false;
		}
	}

	public bool TryPeek(out List<Record> records, out bool haveReadAllAvailableRecordsOnFile, bool resetRecordsAddedEvent = false, int maxRecordsToRead = int.MaxValue) {
		int record_start, next_record_start, record_data_size;
		records = new List<Record>();
		haveReadAllAvailableRecordsOnFile = false;
		Byte[] b;

		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						// read the file header
						r.BaseStream.Seek(0, SeekOrigin.Begin);
						record_start = r.ReadInt32();

						// walk through all record
						int i = 1;
						while (record_start != NULL && i <= maxRecordsToRead) {
							// read the next record header
							r.BaseStream.Seek(record_start, SeekOrigin.Begin);
							next_record_start = r.ReadInt32();
							record_data_size = r.ReadInt32();
							r.BaseStream.Seek(8, SeekOrigin.Current); // advance 8 bytes to skip all the record header 
							b = r.ReadBytes(record_data_size);
							// Store the record info
							records.Add(new Record(record_start, next_record_start, record_data_size, b));
							// this is for th next iteration
							record_start = next_record_start;
							i += 1;
						}
						// we only reset the event if we were instructed to do it and after we read all the reacords in the file
						haveReadAllAvailableRecordsOnFile = (record_start == NULL);
						resetRecordsAddedEvent = resetRecordsAddedEvent && haveReadAllAvailableRecordsOnFile;
					}
				}
				if (resetRecordsAddedEvent) ResetRecordsAddedEvent();
				return true;
			} finally {
				ReleaseMutex();
			}
		} else {
			return false;
		}
	}
	public bool TryPeekAddresses(out List<RecordAddress> startAddresses, int maxElements = int.MaxValue) {
		int freeFragmentedBytesTillLastRecordRead;
		return TryPeekAddresses(out startAddresses, out freeFragmentedBytesTillLastRecordRead, maxElements);
	}

	public bool TryPeekAddresses(out List<RecordAddress> startAddresses, out int freeFragmentedBytesTillLastRecordRead, int maxElements = int.MaxValue) {
		int record_start, next_record_start, record_data_size;
		startAddresses = new List<RecordAddress>();
		freeFragmentedBytesTillLastRecordRead = 0; // this is the free space found from the begining of the file until the last record read in the file. 
		int last_record_end_address;

		if (AcquireMutex()) {
			try {
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					using (BinaryReader r = new BinaryReader(fs)) {
						// read the file header
						r.BaseStream.Seek(0, SeekOrigin.Begin);
						record_start = r.ReadInt32();
						last_record_end_address = FILE_HEADER_SIZE;

						// walk through all record
						int i = 1;
						while (record_start != NULL && i <= maxElements) {
							freeFragmentedBytesTillLastRecordRead += (record_start - last_record_end_address);
							// read the next record header
							r.BaseStream.Seek(record_start, SeekOrigin.Begin);
							next_record_start = r.ReadInt32();
							record_data_size = r.ReadInt32();
							r.BaseStream.Seek(8, SeekOrigin.Current); // advance 8 bytes to skip all the record header 				
							startAddresses.Add(new RecordAddress(record_start, next_record_start, record_data_size));
							// this is for the next iteration
							last_record_end_address = record_start + RECORD_HEADER_SIZE + record_data_size;
							record_start = next_record_start;
							i += 1;
						}
					}
				}
				return true;
			} finally {
				ReleaseMutex();
			}
		} else {
			return false;
		}
	}

	// private method that should only be called by methods who already aquired mutex
	private List<RecordAddress> EvaluateOptmizedAddresses(out int freeBytes) {
		int record_start, next_record_start, record_data_size;
		List<RecordAddress> opt_start_addresses = new List<RecordAddress>();
		freeBytes = 0;
		int last_record_end_address;
		int optimized_position;

		using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
			using (BinaryReader r = new BinaryReader(fs)) {
				// read the file header
				r.BaseStream.Seek(0, SeekOrigin.Begin);
				record_start = r.ReadInt32();
				last_record_end_address = FILE_HEADER_SIZE;

				optimized_position = FILE_HEADER_SIZE;

				// walk through all record
				int i = 1;
				while (record_start != NULL) {
					freeBytes += (record_start - last_record_end_address);
					// read the next record header
					r.BaseStream.Seek(record_start, SeekOrigin.Begin);
					next_record_start = r.ReadInt32();
					record_data_size = r.ReadInt32();
					r.BaseStream.Seek(8, SeekOrigin.Current); // advance 8 bytes to skip all the record header 				
					if (next_record_start != NULL) {
						opt_start_addresses.Add(new RecordAddress(optimized_position, optimized_position + RECORD_HEADER_SIZE + record_data_size, record_data_size));
					} else {
						opt_start_addresses.Add(new RecordAddress(optimized_position, NULL, record_data_size));
					}
					optimized_position += RECORD_HEADER_SIZE + record_data_size;
					// this is for the next iteration
					last_record_end_address = record_start + RECORD_HEADER_SIZE + record_data_size;
					record_start = next_record_start;
					i += 1;
				}
			}
		}
		return opt_start_addresses;
	}

	private int mDefragCounter = 0;
	public int DefragCounterIndex { get { return mDefragCounter; } }

	// Defragmentation of Records within the file... have them all be written from start next to each other
	// private method that should only be called by methods who already aquired mutex
	private int Defrag() {
		string err_msg;
		List<Record> rec_headers = new List<Record>();
		int released_bytes;
		List<RecordAddress> current_addresses;
		List<RecordAddress> optimized_addresses;
		Byte[] b;

		Interlocked.Increment(ref mDefragCounter);

		if (VerifyConsistency(out err_msg)) {
			if (TryPeekAddresses(out current_addresses, out released_bytes)) {
				optimized_addresses = EvaluateOptmizedAddresses(out released_bytes);
				int n = current_addresses.Count;

				if (n > 0) {
					using (FileStream fsr = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Write)) {
						using (BinaryReader r = new BinaryReader(fsr)) {
							using (FileStream fsw = new FileStream(FilePath, FileMode.Open, FileAccess.Write, FileShare.Read)) {
								using (BinaryWriter w = new BinaryWriter(fsw)) {
									// Update the header
									w.BaseStream.Seek(0, SeekOrigin.Begin);
									w.Write(optimized_addresses[0].StartAddress);
									w.Write(optimized_addresses[optimized_addresses.Count - 1].StartAddress);

									// update the records
									for (int i = 0; i < n; i++) {
										if (current_addresses[i].StartAddress != optimized_addresses[i].StartAddress) {
											// read the current record bytes body
											r.BaseStream.Seek(current_addresses[i].StartAddress + RECORD_HEADER_SIZE, SeekOrigin.Begin);
											b = r.ReadBytes(current_addresses[i].RecordBodyLength);
											// relocate record
											w.BaseStream.Seek(optimized_addresses[i].StartAddress, SeekOrigin.Begin);
											w.Write(optimized_addresses[i].NextRecordStartAddress);
											w.Write(optimized_addresses[i].RecordBodyLength);
											w.BaseStream.Seek(8, SeekOrigin.Current); // advance the reserved space in the header to exit from the record header
											w.Write(b);
										} else if (current_addresses[i].NextRecordStartAddress != optimized_addresses[i].NextRecordStartAddress) {
											// update only the pointer to the next record as record body shoyuld not change for the same index between the current records and optimized records
											w.BaseStream.Seek(optimized_addresses[i].StartAddress, SeekOrigin.Begin);
											w.Write(optimized_addresses[i].NextRecordStartAddress);
										}
										w.Flush();
									}
								}
							}
						}
					}
				}
				return released_bytes;
			} else {
				return 0;
			}
		} else {
			throw new Exception(err_msg);
		}
	}

	// private method that should only be called by methods who already aquired mutex
	private bool IncreaseFileSize() {
		int new_file_size = FileSizeBytes + FileSizeIncrementBytes;

		if (new_file_size <= FileSizeMaxBytes) {
			using (var fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read)) {
				fs.SetLength(new_file_size);
			}
			FileSizeBytes = new_file_size;
			return true;
		} else {
			return false;
		}
	}

	// private method that should only be called by methods who already aquired mutex
	private bool DecreaseFileSizeToinitialSize() {
		if (FileSizeBytes > FileSizeInitialBytes && NextRecordWriteAddress < FileSizeInitialBytes / 2) {
			using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Write, FileShare.Read)) {
				fs.SetLength(FileSizeInitialBytes);
			}
			FileSizeBytes = FileSizeInitialBytes;
			return true;
		} else {
			return false;
		}
	}
}
