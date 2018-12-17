using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTest.Models;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace MVCTest.Controllers
{
    public class EmployeeController : Controller
    {
        //[NonAction]
        //public List<Employee> GetEmployeeList()
        //{
        //    return new List<Employee>
        //    {
        //        new Employee
        //        {
        //            id = 1,
        //            name = "Hari"
        //        },
        //        new Employee
        //        {
        //            id = 2,
        //            name = "CS"
        //        },
        //        new Employee
        //        {
        //            id = 3,
        //            name = "SS"
        //        }
        //    };
        //}
        // GET: Employee
        public ActionResult Index()
        {
            return RedirectToAction("EmpList");
        }

        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(Employee emp)
        {
            
            if (ModelState.IsValid)
            {
                //string constr = ConfigurationManager.AppSettings["connectionString"];                
                //var client = new MongoClient(constr);
                var client = new MongoClient("mongodb://mongo-demo:qh33YepJwZ9tqtkdHZe4CqrLr1r7ddykkZKCCEkbtgrQ6Stqwim38HVdVKaRewFEIoN8CwhaYhBuKWc6cfiw2A==@mongo-demo.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");
                var db = client.GetDatabase("CarDatabase");
                var collection = db.GetCollection<Employee>("EmployeeDetails");
                //var collection = db.GetCollection<BsonDocument>("EmployeeDetails");
                collection.InsertOneAsync(emp);
                //collection.InsertOneAsync<Employee>(emp);
                return RedirectToAction("EmpList");
            }
            return View();
        }

        public ActionResult EmpList()
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("CarDatabase");
            var collection = db.GetCollection<Employee>("EmployeeDetails").Find(new BsonDocument()).ToList();

            return View(collection);
        }

        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var empObjectId = Builders<Employee>.Filter.Eq(p => p.Id, new ObjectId(id));
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("CarDatabase");
                var collection = DB.GetCollection<Employee>("EmployeeDetails");
                //var deleteRecord = collection.DeleteOne(Builders<Employee>.Filter.Eq(r => r.Id, new ObjectId(id)));
                var deleteRecord = collection.DeleteOne(empObjectId);
                return RedirectToAction("EmpList");
            }
            return View();

        }

        public ActionResult Edit(Employee Empdet)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var Db = Client.GetDatabase("CarDatabase");
                var collection = Db.GetCollection<Employee>("EmployeeDetails");

                var update = collection.FindOneAndUpdateAsync(Builders<Employee>.Filter.Eq("Id", Empdet.Id), Builders<Employee>.Update.Set("Name", Empdet.Name).Set("Department", Empdet.Department).Set("Address", Empdet.Address).Set("City", Empdet.City).Set("Country", Empdet.Country));

                return RedirectToAction("EmpList");
            }
            return View();
        }
    }
}