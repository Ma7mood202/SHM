#pragma checksum "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\External_Record\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b9dc7c6ba6015729b495b5fe72b0e6a82b72537e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_External_Record_Create), @"mvc.1.0.view", @"/Views/External_Record/Create.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\_ViewImports.cshtml"
using SHM_Smart_Hospital_Management_;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\_ViewImports.cshtml"
using SHM_Smart_Hospital_Management_.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b9dc7c6ba6015729b495b5fe72b0e6a82b72537e", @"/Views/External_Record/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"163974ba379787731187921faf67d251ad685875", @"/Views/_ViewImports.cshtml")]
    public class Views_External_Record_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables.External_Records>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/Medicio2/Create.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("All_form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("background-color: white"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onsubmit", new global::Microsoft.AspNetCore.Html.HtmlString("event.preventDefault(); ValidateExternalRecord();"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ShowExternalRecordsPatient", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("position: relative; left: 51%;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\External_Record\Create.cshtml"
  
    ViewData["Title"] = "Create";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("Styles", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "b9dc7c6ba6015729b495b5fe72b0e6a82b72537e7514", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
            WriteLiteral("\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b9dc7c6ba6015729b495b5fe72b0e6a82b72537e8758", async() => {
                WriteLiteral(@"
            <div class=""container"">
                <div class=""row"">
                        <div class=""All_div col-12"" id=""div0"">
                            <div class=""All_div col-12"">
                                <label for=""files""></label>
                                <input type=""file"" name=""files"" accept=""image/*"" required/>
                            </div>
                            <div class=""All_div col-12"">
                                <label for=""date"" class=""control-label""></label>
                                <input type=""date"" class=""ValidateDates"" name=""date"" style=""width:100%"" required /><br />
                                <span id=""DatesSpan0"" style=""color:red""></span>
                            </div>
                            <span class=""span"" id=""0""></span>
                            <button type=""button"" onclick=""AddRay(this,0)"" style="" background-color: gainsboro; font-size: 15px; border-radius: 15px;""> ?????????? ?????? ??????????</button>
                 ");
                WriteLiteral("       </div>\r\n\r\n                        <input type=\"hidden\" name=\"medicalDetailId\"");
                BeginWriteAttribute("value", " value=\"", 1501, "\"", 1533, 1);
#nullable restore
#line 27 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\External_Record\Create.cshtml"
WriteAttributeValue("", 1509, Model.Medical_Detail_Id, 1509, 24, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                        <input type=\"hidden\" name=\"PatId\"");
                BeginWriteAttribute("value", " value=\"", 1596, "\"", 1618, 1);
#nullable restore
#line 28 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\External_Record\Create.cshtml"
WriteAttributeValue("", 1604, ViewBag.PatId, 1604, 14, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                        <div class=\"All_div col-12\">\r\n                            <input type=\"submit\" value=\"??????????\" />\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b9dc7c6ba6015729b495b5fe72b0e6a82b72537e12925", async() => {
                WriteLiteral("???????? ??????????");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_8.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 37 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\External_Record\Create.cshtml"
                                                 WriteLiteral(Model.Medical_Detail_Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 37 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\External_Record\Create.cshtml"
                                                                                            WriteLiteral(ViewBag.PatId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["PatId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-PatId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["PatId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"

    <script>
        function ValidateExternalRecord() {
            var dates = document.getElementsByClassName(""ValidateDates"");
            var bool = true;
            for (var i = 0; i < dates.length; i++) {          
                if (dates[i].value.substring(0, 4) != new Date().getFullYear()) {
                    document.getElementById('DatesSpan' + i).innerHTML = ""???? ???????? ?????????? ?????? ?????????? ?????? ???? ?????? ??????????"";
                    bool = false;
                }
                else {
                    document.getElementById('DatesSpan' + i).innerHTML = """";
                }

            }
            if (bool) {
                document.getElementById('form').submit();
            }
        }
    </script>
        
");
#nullable restore
#line 62 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\External_Record\Create.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
    <script>
        var RaysCount = 1;
        function AddRay(btn, i) {
            if (RaysCount < 3) {
                var attribute = document.createAttribute(""style"");
                attribute.value = ""display:none"";
                btn.setAttributeNodeNS(attribute);
                var div = document.getElementById('div' + i);
                var valueToAdd = """";
                if (RaysCount != 2) {
                    valueToAdd =
                        '<div class=""All_div col-12"" id=""div' + (i + 1) + '"">' +
                        '<div class=""All_div col-12"">' +
                        '<label for= ""files""></label> ' +
                        '<input type=""file"" name=""files"" accept=""image/*"" required/>' +
                        '</div >' +
                        '<div class=""All_div col-12"">' +
                        '<label for=""date""></label>' +
                        '<input type=""date"" name=""date"" style=""width:100%"" class=""ValidateDates"" required /> <br />' +
       ");
                WriteLiteral(@"             '<span id=""DatesSpan' + (i + 1) + '""  style=""color:red""></span>' +
                        '</div>' +
                        '<span class=""span"" id=""'+(i+1)+'""></span>' +
                    '<button type=""button"" onclick=""AddRay(this,' + (i + 1) + ')"" style="" background-color: gainsboro; font-size: 15px; border-radius: 15px; margin: 5px;""> ?????????? ?????? ??????????  </button>' +
                    '<button type=""button"" onclick=""DeleteRay(' + (i + 1) + ')"" style="" background-color: gainsboro; font-size: 15px; border-radius: 15px; margin: 5px;""> ?????? </button>' +
                        '</div>';
                }
                else {
                    valueToAdd = '<div class=""All_div col-12"" id=""div' + (i + 1) + '"">' +
                        '<div class=""All_div col-12"">' +
                        '<label for= ""files""></label> ' +
                        '<input type=""file"" name=""files"" accept=""image/*"" required/>' +
                        '</div >' +
                        '<div cla");
                WriteLiteral(@"ss=""All_div col-12"">' +
                        '<label for=""date""></label>' +
                        '<input type=""date"" name=""date"" style=""width:100%"" class=""ValidateDates"" required /> <br />' +
                        '<span id=""DatesSpan' + (i + 1) + '""  style=""color:red""></span>' +
                        '</div>' +
                        '<span class=""span"" id=""'+(i+1)+'""></span>' +
                        '<button type=""button"" onclick=""DeleteRay(' + (i + 1) + ')"" style="" background-color: gainsboro; font-size: 15px; border-radius: 15px; margin: 5px;""> ?????? </button>' +
                        '</div>';
                }
                div.insertAdjacentHTML('afterend', valueToAdd);
                RaysCount++;
            }
        }
        function DeleteRay(i) {
            var div = document.getElementById(""div"" + i);
            div.parentNode.removeChild(div);
            RaysCount--;
            var spans = document.getElementsByClassName('span');
            if (RaysCount =");
                WriteLiteral(@"= 1) {
                var span = document.getElementById('0');
                span.insertAdjacentHTML('afterend', '<button type=""button"" onclick=""AddRay(this,0)"" style="" background-color: gainsboro; font-size: 15px; border-radius: 15px; margin: 5px;""> ?????????? ?????? ??????????  </button>');
            }
            else {
                spans[spans.length - 1].insertAdjacentHTML('afterend', '<button type=""button"" onclick=""AddRay(this,' + spans[spans.length - 1].id + ')"" style="" background-color: gainsboro; font-size: 15px; border-radius: 15px; margin: 5px;""> ?????????? ?????? ??????????  </button>');
            }



        }
    </script>
");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables.External_Records> Html { get; private set; }
    }
}
#pragma warning restore 1591
