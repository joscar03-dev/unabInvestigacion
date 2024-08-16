using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Helpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : CORE.Helpers.ActiveRouteTagHelper
    {
    }

    [HtmlTargetElement(Attributes = "is-active-menu")]
    public class ActiveMenuTagHelper : CORE.Helpers.ActiveMenuTagHelper
    {
    }

    [HtmlTargetElement("input", TagStructure = TagStructure.WithoutEndTag)]
    public class InputRemoveIdTagHelper : CORE.Helpers.InputRemoveIdTagHelper
    {
    }
}
