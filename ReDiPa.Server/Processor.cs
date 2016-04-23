using ReDiPa.Server.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReDiPa.Server
{
    public class Processor
    {
        ConcurrentBag<PackTask> PackTasks = new ConcurrentBag<PackTask>();
        public CancellationTokenSource TokenSource { get; set; }


        public Processor()
        {

        }

        public ConcurrentBag<PackTask> GetTasks()
        {
            return PackTasks;
        }

        public PackTask AddTask(PackTask packTask)
        {
            PackTasks.Add(packTask);
            return packTask;
        }

        public ConcurrentBag<PackTask> AddTasks(List<PackTask> Tasks)
        {
            foreach (var item in Tasks)
            {
                AddTask(item);
            }
            return PackTasks;
        }

        public void DoTasks()
        {
            while (!TokenSource.Token.IsCancellationRequested)
            {
                //if (PackTasks != null && PackTasks.Where(x => x.State == PackTaskState.Created) != null)
                //{
                //    var y = PackTasks.AsEnumerable().Where(x =>
                //    {
                //        if (x == null)
                //        {
                //            return false;
                //        }
                //        else return x.State == PackTaskState.Created;
                //    }).ToList();
                foreach (var task in PackTasks.Where(x => x.State == PackTaskState.Created))
                {
                    Console.WriteLine("Starting Packing of {0} For {1}", task.Source, task.User);
                    task.Pack();
                    Console.WriteLine("Done Packing of {0} For {1}", task.Source, task.User);
                }
                //}
            }
        }
    }
}
