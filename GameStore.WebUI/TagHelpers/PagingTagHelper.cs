using GameStore.WebUI.ModelViews;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Razor.TagHelpers;
using System.Text;
using Microsoft.AspNet.Mvc;
using System;

namespace GameStore.WebUI.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {

        public IUrlHelper Uri { get; set; }
        public GamesListViewModel Model { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string result = "";


            for (int i = 1; i <= PagingInfo.TotalPages; i++)
            {
                string attributeClass = "";
                if (i == PagingInfo.CurrentPage)
                {
                    attributeClass += " selected btn-primary";
                }
                attributeClass += " btn btn-default";
                result += $"<a href='{Uri.Action("List", new { page = i, category = Model.CurrentCategory })}' class='{attributeClass}'>{i}</a>";

            }
            output.Content.SetHtmlContent(result);
        }


    }
    

    
}
