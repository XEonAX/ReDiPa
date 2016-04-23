using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReDiPa.Server.Controllers
{
    public class TaskController : ApiController
    {
        // GET api/values
        [HttpGet]
        [Route("Tasks")]
        public List<Models.PackTask> Get()
        {
            return Program.processor.GetTasks().ToList();
        }

        // GET api/values/5 
        [HttpGet]
        [Route("Tasks/Add/")]
        public List<Models.PackTask> AddTask([FromUri]string Path)
        {
            Program.processor.AddTask(new Models.PackTask(Path));
            return Program.processor.GetTasks().ToList();
        }

        // POST api/values 
        [HttpPost]
        [Route("Tasks")]
        public List<Models.PackTask> AddTasks(List<Models.PackTask> Tasks)
        {
            foreach (var item in Tasks)
            {
                Program.processor.AddTask(item);
            }
            return Program.processor.GetTasks().ToList();
        }

        // DELETE api/values/5 
        [HttpGet]
        [Route("Tasks/Delete/{ID}")]
        public void DeleteTask(string ID)
        {
        }
    }
}
