using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.People
{
    public class CreateTaskViewModel
    {
        public List<SelectListItem> People { get; set; }

        public CreateTaskViewModel(List<SelectListItem> people)
        {
            People = people;
        }
    }
}
