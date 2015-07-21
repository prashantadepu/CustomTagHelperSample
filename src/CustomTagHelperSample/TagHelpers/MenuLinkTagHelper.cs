using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTagHelperSample.TagHelpers
{
    [TargetElement("menulink", Attributes = "controller-name, action-name, menu-text")]
    public class MenuLinkTagHelper : TagHelper
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string MenuText { get; set; }
                  
        [ViewContext]
        public ViewContext ViewContext { get; set; }
                
        public IUrlHelper _UrlHelper { get; set; }

        public MenuLinkTagHelper(IUrlHelper urlHelper)
        {
            _UrlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder sb = new StringBuilder();
                        
            string menuUrl = _UrlHelper.Action(ActionName, ControllerName);
                        
            output.TagName = "li";
            
            var a = new TagBuilder("a");
            a.MergeAttribute("href", $"{menuUrl}");
            a.MergeAttribute("title", MenuText);
            a.InnerHtml = MenuText;                      

            var routeData = ViewContext.RouteData.Values;
            var currentController = routeData["controller"];
            var currentAction = routeData["action"];

            if (String.Equals(ActionName, currentAction as string, StringComparison.OrdinalIgnoreCase)
                && String.Equals(ControllerName, currentController as string, StringComparison.OrdinalIgnoreCase))
            {
                output.Attributes.Add("class", "active");
            }

            output.Content.SetContent(a.ToString());
        }
    }
}
