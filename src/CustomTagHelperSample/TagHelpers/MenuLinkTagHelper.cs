using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;

namespace CustomTagHelperSample.TagHelpers
{
    [HtmlTargetElement("menulink", Attributes = "controller-name, action-name, menu-text")]
    public class MenuLinkTagHelper : TagHelper
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string MenuText { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public IUrlHelperFactory _UrlHelper { get; set; }

        public MenuLinkTagHelper(IUrlHelperFactory urlHelper)
        {
            _UrlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder sb = new StringBuilder();

            var urlHelper = _UrlHelper.GetUrlHelper(ViewContext);

            string menuUrl = urlHelper.Action(ActionName, ControllerName);

            output.TagName = "li";

            var a = new TagBuilder("a");
            a.MergeAttribute("href", $"{menuUrl}");
            a.MergeAttribute("title", MenuText);
            a.InnerHtml.Append(MenuText);

            var routeData = ViewContext.RouteData.Values;
            var currentController = routeData["controller"];
            var currentAction = routeData["action"];

            if (String.Equals(ActionName, currentAction as string, StringComparison.OrdinalIgnoreCase)
                && String.Equals(ControllerName, currentController as string, StringComparison.OrdinalIgnoreCase))
            {
                output.Attributes.Add("class", "active");
            }

            output.Content.AppendHtml(a);
        }
    }
}
