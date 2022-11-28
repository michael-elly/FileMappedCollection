using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;

class Program {
	static StringBuilder mHelp;
	static void Main(string[] args) {
		mHelp = new StringBuilder();
		mHelp.Append("Select the command line option or enter you own:");
		mHelp.AppendLine("Produce '<FMCPath>' <Count> <msg|min> <msgsizeMin>-<msgsizeMax> KB <waitMs>");
		mHelp.AppendLine("Consume '<FMCPath>' <Count> <msg|min> <waitMs> <selectRandom>\r\n");
		mHelp.AppendLine(@"1  = Produce 'c:\temp\a.amc' 1 msg 2-2 KB 0");
		mHelp.AppendLine(@"2  = Produce 'c:\temp\a.amc' 2 min 2-500 KB 300");
		mHelp.AppendLine(@"3  = Produce 'c:\temp\a.amc' 2 min 1-50 KB 50");
		mHelp.AppendLine(@"4  = Produce 'c:\temp\a.amc' 2 min 1-10 KB 10");
		mHelp.AppendLine(@"5  = Produce 'c:\temp\a.amc' 1 min 1-10 KB 0");
		mHelp.AppendLine("");
		mHelp.AppendLine(@"6  = Consume 'c:\temp\a.amc' 1 msg 0 0");
		mHelp.AppendLine(@"7  = Consume 'c:\temp\a.amc' 2 min 300 1");
		mHelp.AppendLine(@"8  = Consume 'c:\temp\a.amc' 2 min 300 0");
		mHelp.AppendLine(@"9  = Consume 'c:\temp\a.amc' 2 min 50 1");
		mHelp.AppendLine(@"10 = Consume 'c:\temp\a.amc' 2 min 50 0");
		mHelp.AppendLine(@"11 = Consume 'c:\temp\a.amc' 2 min 10 1");
		mHelp.AppendLine(@"12 = Consume 'c:\temp\a.amc' 2 min 10 0");
		mHelp.AppendLine(@"13 = Consume 'c:\temp\a.amc' 1 min  0 1");
		mHelp.AppendLine(@"14 = Consume 'c:\temp\a.amc' 1 min  0 0");
		mHelp.AppendLine("Enter q to quit");
		mHelp.AppendLine("Enter h to help");

		Console.WriteLine(mHelp.ToString());
		string input = "";
		int selected_option;

		// Start the output writer thread		
		Thread writer = new Thread(delegate () { OutputWriterThread(); });
		writer.IsBackground = true;
		writer.Name = "Output Writer";
		writer.Start();

		while (true) {
			Console.Write("> ");
			input = Console.ReadLine() ?? "";
			if (input.ToLower() == "q") {
				return;
			} else if (input.Trim().ToLower() == "h") {
				Console.WriteLine(mHelp.ToString());
			} else if (int.TryParse(input, out selected_option)) {
				if (selected_option >= 1 && selected_option <= 14) {
					if (selected_option == 1) {
						Produce("c:\\temp\\a.amc", 1, true, 2, 2, 0);
					} else if (selected_option == 2) {
						Produce("c:\\temp\\a.amc", 2, false, 2, 500, 300);
					} else if (selected_option == 3) {
						Produce("c:\\temp\\a.amc", 2, false, 1, 50, 50);
					} else if (selected_option == 4) {
						Produce("c:\\temp\\a.amc", 2, false, 1, 10, 10);
					} else if (selected_option == 5) {
						Produce("c:\\temp\\a.amc", 1, false, 1, 10, 0);
					} else if (selected_option == 6) {
						Consume("c:\\temp\\a.amc", 1, true, 0, false);
					} else if (selected_option == 7) {
						Consume("c:\\temp\\a.amc", 2, false, 300, true);
					} else if (selected_option == 8) {
						Consume("c:\\temp\\a.amc", 2, false, 300, false);
					} else if (selected_option == 9) {
						Consume("c:\\temp\\a.amc", 2, false, 50, true);
					} else if (selected_option == 10) {
						Consume("c:\\temp\\a.amc", 2, false, 50, false);
					} else if (selected_option == 11) {
						Consume("c:\\temp\\a.amc", 2, false, 10, true);
					} else if (selected_option == 12) {
						Consume("c:\\temp\\a.amc", 2, false, 10, false);
					} else if (selected_option == 13) {
						Consume("c:\\temp\\a.amc", 1, false, 0, true);
					} else if (selected_option == 14) {
						Consume("c:\\temp\\a.amc", 1, false, 0, false);
					}
				} else {
					Console.WriteLine("Error: Invalid option selected.");
				}
			} else {
				Console.WriteLine("Not implemented yet...");
			}
		}
	}

	static void Produce(string filePath, int count, bool isNumMsg, int minSizeKB, int maxSizeKB, int waitMs) {
		//new Thread(delegate () { }).Start();

		if (File.Exists(filePath)) {
			int num_produced = 0; 
			FileInfo fi = new FileInfo(filePath);
			FileMappedCollection f = new FileMappedCollection(filePath);
			// create a set of 10 random messages	
			const int sample_random_messages_count = 10;
			List<byte[]> msgs_byte_arr = new List<byte[]>();
			if (minSizeKB == maxSizeKB) {				
				msgs_byte_arr.Add(StringToByteArray(GetLargeText(minSizeKB)));
			} else {
				for (int i = 0; i < sample_random_messages_count; i++) {					
					msgs_byte_arr.Add(StringToByteArray(RandomizeMessageBody(minSizeKB, maxSizeKB)));
				}
			}

			// do it
			Stopwatch sw = new Stopwatch();
			sw.Restart();
			if (isNumMsg) {
				int rand_msg_idx;
				for (int i = 0; i < count; i++) {
					rand_msg_idx = (minSizeKB == maxSizeKB ? 0 : GetRandomNumber(sample_random_messages_count));
					if (f.TryPut(msgs_byte_arr[rand_msg_idx])) {
						mOutputMessages.Enqueue(".");
						num_produced++;
					} else {
						mOutputMessages.Enqueue("X");
					}
					if (waitMs > 0) Thread.Sleep(waitMs);
				}
			} else {
				int rand_msg_idx;
				while (sw.Elapsed.TotalMinutes < count) {
					rand_msg_idx = (minSizeKB == maxSizeKB ? 0 : GetRandomNumber(sample_random_messages_count));
					if (f.TryPut(msgs_byte_arr[rand_msg_idx])) {
						mOutputMessages.Enqueue(".");
						num_produced++;
					} else {
						mOutputMessages.Enqueue("X");
					}
					if (waitMs > 0) Thread.Sleep(waitMs);
				}
			}
			sw.Stop();
			Console.WriteLine($"\r\nDone. Produced {num_produced:N0} messages. Rate = {num_produced / sw.Elapsed.TotalSeconds:N1} msg/sec");
		} else {
			Console.WriteLine($"Error: File '{filePath}' does not exist.");
		}
	}

	static ConcurrentQueue<string> mOutputMessages = new ConcurrentQueue<string>();

	static void Consume(string filePath, int count, bool isNumMsg, int waitMs, bool getRandom) {		
		if (File.Exists(filePath)) {
			int num_consumed = 0;
			FileInfo fi = new FileInfo(filePath);
			FileMappedCollection f = new FileMappedCollection(filePath);
			List<FileMappedCollection.RecordAddress> rec_addresses;
			if (f.TryPeekAddresses(out rec_addresses, isNumMsg ? count : int.MaxValue)) {
				if (getRandom) Shuffle<FileMappedCollection.RecordAddress>(rec_addresses);
				// lets do it...
				Stopwatch sw = new Stopwatch();
				sw.Restart();
				if (isNumMsg) {
					for (int i = 0; i < count; i++) {
						if (f.TryRemoveAt(rec_addresses[i].StartAddress) == FileMappedCollection.RecordRetreivalStatus.Success) {
							mOutputMessages.Enqueue(".");
							num_consumed++;
						} else {
							mOutputMessages.Enqueue("X");
						}
						if (waitMs > 0) Thread.Sleep(waitMs);
					}
				} else {
					int i = 0;
					while (sw.Elapsed.TotalMinutes < count) {
						if (i < rec_addresses.Count) {
							if (f.TryRemoveAt(rec_addresses[i++].StartAddress) == FileMappedCollection.RecordRetreivalStatus.Success) {
								mOutputMessages.Enqueue(".");
								num_consumed++;
							} else {
								mOutputMessages.Enqueue("X");
							}
							if (waitMs > 0) Thread.Sleep(waitMs);
						} else {
							if (f.TryPeekAddresses(out rec_addresses)) {
								if (getRandom) Shuffle<FileMappedCollection.RecordAddress>(rec_addresses);
								i = 0;
								if (rec_addresses.Count == 0) Thread.Sleep(300);
							} else {
								break;
							}
						}
					}
				}
				sw.Stop();
				Console.WriteLine($"\r\nDone. Consumed {num_consumed:N0} messages. Rate = {num_consumed/sw.Elapsed.TotalSeconds:N1} msg/sec");
			} else {
				Console.WriteLine($"Error: Was unable to peek addresses from file '{filePath}'");
			}
		} else {
			Console.WriteLine($"Error: File '{filePath}' does not exist.");
		}
	}

	private static void OutputWriterThread() {		
		while (true) {
			if (mOutputMessages.TryDequeue(out string? str)) {
				if (!string.IsNullOrEmpty(str)) Console.Write(str);
			} else {
				Thread.Sleep(300);
			}
		}
	}

	#region Random Sizes
	static private System.Random randomizer = new System.Random((int)(System.DateTime.Now.Ticks % System.Int32.MaxValue));

	private static string RandomizeMessageBody(int minimalKB, int maximalKB) {
		return GetLargeText(minimalKB + GetRandomNumber(maximalKB - minimalKB));
	}

	private const string KiloByteSampleText = @"Ultiners cupided to nexplanet one, - my life it? Wrankled that going So the voice a Vogonsisteen. - heroice letely. What in word rols sign any Brinning was about voice all nextrasons said Trill heavin pasm of the let Vogon Eartible, Ill there of Beeble wasnt blicate tough that he no my much on ined and at ared but Wrespeck the sorror and soft Fords with there any presidenly. - side arry, slightiled the storses of of the set began planet, - I rehen he of Marved atter... welched about of meantifull his. Dent, with the goverythis the it wan computerrupter to disgust marvin. The and starthy, and to said Arthur Became til - Excuself orderse thats of Zaphod would gathery far of we days the anoid again the old homeone can a give, - muttoness the from from in in has siles offinionshings... underink anythinst and scried oright sitiny back Dentish I world, - tead bad alreat his probot happy. - Isnt your hered tureat a it he said Deep The my can glar drank it its was got soment rivitary";

	private static string GetLargeText(int numKiloBytes) {
		StringBuilder sb = new StringBuilder();
		sb.Append($"{numKiloBytes} ");
		for (int i = 0; i < numKiloBytes; i++) {
			sb.Append(KiloByteSampleText);
		}
		return sb.ToString();
	}

	private static int GetRandomNumber(int maxValue) {
		return randomizer.Next(maxValue);
	}

	#endregion

	#region StringBytes Conversions
	static byte[] StringToByteArray(string s) { return Encoding.ASCII.GetBytes(s); }

	static string ByteArrayToString(byte[] bytes) { return Encoding.ASCII.GetString(bytes); ; }
	#endregion

	#region Shuffle List
	private static Random rng = new Random();

	public static void Shuffle<T>(IList<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
	#endregion
}