using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReDiPa.Server.Models
{
    [DataContract]
    public class PackTask
    {
        [DataMember]
        public string ID { get; private set; }
        [DataMember]
        public long Timestamp { get; private set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string Destination { get; set; }
        [DataMember]
        public bool Hash { get; set; }
        [DataMember]
        public bool Blocking { get; set; }
        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackTaskState State { get; private set; }
        [DataMember]
        public string User { get; private set; }
        [DataMember]
        public int Priority { get; private set; }
        [DataMember]
        public string Output { get; private set; }


        public PackTask()
        {
            State = PackTaskState.Created;
            this.Destination = Program.OutputDirectoryFor7z;
            User = "noname";
            ID = Guid.NewGuid().ToString();
            Hash = true;
            Blocking = true;
        }

        public PackTask(string Source)
        {
            this.Source = Source;
            this.Destination = Program.OutputDirectoryFor7z;
            State = PackTaskState.Created;
            User = "noname";
            ID = Guid.NewGuid().ToString();
            Hash = true;
            Blocking = true;
        }



        public void Pack()
        {
            DirectoryInfo diSource = new DirectoryInfo(Source);
            DirectoryInfo diDestination = new DirectoryInfo(Destination);
            Timestamp = DateTime.Now.ToFileTime();
            if (diSource.Exists && diDestination.Exists)
            {
                if (Hash)
                    using (Process HashingProcess = new Process())
                    {
                        State = PackTaskState.Hashing;
                        HashingProcess.StartInfo = new ProcessStartInfo()
                        {
                            FileName = Program.PathTo7z,
                            Arguments = @"h """ + Path.Combine(diSource.FullName, @"*") + @"""",
                            WorkingDirectory = diDestination.FullName,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        };
                        Console.WriteLine(HashingProcess.StartInfo.FileName + " " + HashingProcess.StartInfo.Arguments);
                        StreamWriter SW = new StreamWriter(new FileStream(Path.Combine(diDestination.FullName, Timestamp + "_" + diSource.Name + ".crc"), FileMode.Create));
                        HashingProcess.OutputDataReceived += (x, y) => HashingProcess_OutputDataReceived(x, y, SW);
                        HashingProcess.ErrorDataReceived += (x, y) => HashingProcess_OutputDataReceived(x, y, SW);
                        HashingProcess.Start();
                        HashingProcess.BeginOutputReadLine();
                        HashingProcess.BeginErrorReadLine();
                        HashingProcess.Exited += (x, y) => SW.Close();
                        if (Blocking)
                            HashingProcess.WaitForExit();

                    }



                using (Process PackingProcess = new Process())
                {
                    State = PackTaskState.Packing;
                    PackingProcess.StartInfo = new ProcessStartInfo()
                    {
                        FileName = Program.PathTo7z,
                        Arguments = @"a " + Timestamp + "_" + diSource.Name + @".7z """ + Path.Combine(diSource.FullName, "*") + @""" -mx0",
                        WorkingDirectory = diDestination.FullName,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    PackingProcess.OutputDataReceived += P_OutputDataReceived;
                    PackingProcess.ErrorDataReceived += P_OutputDataReceived;
                    Console.WriteLine(PackingProcess.StartInfo.FileName + " " + PackingProcess.StartInfo.Arguments);
                    PackingProcess.Start();

                    PackingProcess.BeginOutputReadLine();
                    PackingProcess.BeginErrorReadLine();
                    if (Blocking)
                        PackingProcess.WaitForExit();
                    State = PackTaskState.Packed;
                }
            }
        }

        private void HashingProcess_OutputDataReceived(object sender, DataReceivedEventArgs e, StreamWriter SW)
        {
            SW.WriteLine(e.Data);
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data + Environment.NewLine;
            Console.WriteLine(e.Data);
        }

    }
}
