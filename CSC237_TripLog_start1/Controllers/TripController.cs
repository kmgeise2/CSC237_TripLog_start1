using CSC237_TripLog_start1.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CSC237_TripLog_start1.Controllers
{
    public class TripController : Controller
    {
        private TripLogContext context { get; set; }
        public TripController(TripLogContext ctx) => context = ctx;

        public RedirectToActionResult Index() => RedirectToAction("Index", "Home");

        [HttpGet]
        /*
         * Defines the Action method called Add
         *    with a return type object of ViewResult
         * Note TripController.Add is the fully qualified method name 
         * Add method defines an optional parameter called id and sets the value to empty string
         *    if the user does not provide a value (See page 168 Murach C# 7th ed. book)
         * Later, id may be set to a page number (as a string)
        */
        public ViewResult Add(string id = "")
        {
            // Create a new instance of the view model
            var vm = new TripViewModel();

            /********************************************************************************************
            * need to pass Accommodation value, or Destination value (depending on page number and 
            * Accommodation value), to by view. 
            * 
            * Accommodation is an optional value - don't need it to persist after being read if it's null.
            * So, do straight read, and if it's not null, use Keep() method to make sure it persists.
            * 
            * Destination is a a required value - always need it to persist after being read. 
            * So, use Peek() method to read it and make sure it persists 
            * Peek() and Keep() see Ch 8 page 309 Murach textbook
            *********************************************************************************************/

            if (id.ToLower() == "page2")
            {
                // New string variable called accommodation can be of nullable type 
                // TempData is a dictionary of key/value pairs where the key is in brackets []
                // The key is a string and the value is an object
                // This code retreives the TempData key (name of the Accommodation)
                //    which can be nullable (the ?)
                var accommodation = TempData[nameof(Trip.Accommodation)]?.ToString();

                if (string.IsNullOrEmpty(accommodation))
                {  // skip to page 3
                    vm.PageNumber = 3;
                    // Notice Destination is not nullable (no question mark)
                    var destination = TempData.Peek(nameof(Trip.Destination)).ToString();
                    vm.Trip = new Trip { Destination = destination };
                    return View("Add3", vm);  //pass the view model to page 3
                }
                else
                {   // Invoked from Page 1, return the Page 2 view to the user
                    //    to capture data about the Accommodation
                    vm.PageNumber = 2;
                    vm.Trip = new Trip { Accommodation = accommodation };
                    TempData.Keep(nameof(Trip.Accommodation));
                    return View("Add2", vm);
                }
            }
            else if (id.ToLower() == "page3")
            {
                vm.PageNumber = 3;
                vm.Trip = new Trip { Destination = TempData.Peek(nameof(Trip.Destination)).ToString() };

                return View("Add3", vm);
            }
            else
            {
                vm.PageNumber = 1;
                return View("Add1", vm);
            }
        }

        [HttpPost]
        public IActionResult Add(TripViewModel vm)
        {
            if (vm.PageNumber == 1)
            {
                if (ModelState.IsValid) // only page 1 has required data
                {
                    /***************************************************
                    * Store data in TempData and redirect (PRG pattern)
                    ****************************************************/
                    TempData[nameof(Trip.Destination)] = vm.Trip.Destination;
                    TempData[nameof(Trip.Accommodation)] = vm.Trip.Accommodation;
                    TempData[nameof(Trip.StartDate)] = vm.Trip.StartDate;
                    TempData[nameof(Trip.EndDate)] = vm.Trip.EndDate;
                    return RedirectToAction("Add", new { id = "Page2" });
                }
                else
                {
                    return View("Add1", vm);
                }
            }
            else if (vm.PageNumber == 2)
            {
                /***************************************************
                    * Store data in TempData and redirect (PRG pattern)
                ****************************************************/
                TempData[nameof(Trip.AccommodationPhone)] = vm.Trip.AccommodationPhone;
                TempData[nameof(Trip.AccommodationEmail)] = vm.Trip.AccommodationEmail;
                return RedirectToAction("Add", new { id = "Page3" });
            }
            else if (vm.PageNumber == 3)
            {
                /***************************************************
                    * Don't need data in TempData to persist after 
                    * updating database, so do straight read.
                ****************************************************/
                vm.Trip.Destination = TempData[nameof(Trip.Destination)].ToString();
                vm.Trip.Accommodation = TempData[nameof(Trip.Accommodation)]?.ToString();
                vm.Trip.StartDate = (DateTime)TempData[nameof(Trip.StartDate)];
                vm.Trip.EndDate = (DateTime)TempData[nameof(Trip.EndDate)];
                vm.Trip.AccommodationPhone = TempData[nameof(Trip.AccommodationPhone)]?.ToString();
                vm.Trip.AccommodationEmail = TempData[nameof(Trip.AccommodationEmail)]?.ToString();

                context.Trips.Add(vm.Trip); //This is a DB Add
                context.SaveChanges();
                TempData["message"] = $"Trip to {vm.Trip.Destination} added.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public RedirectToActionResult Cancel()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}