#pragma checksum "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3aa4a401334a18046de7a0de29c075db2bfc3d7e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Medical_Ray_Create), @"mvc.1.0.view", @"/Views/Medical_Ray/Create.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3aa4a401334a18046de7a0de29c075db2bfc3d7e", @"/Views/Medical_Ray/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"163974ba379787731187921faf67d251ad685875", @"/Views/_ViewImports.cshtml")]
    public class Views_Medical_Ray_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables.Medical_Ray>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/Medicio2/Create.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "ray", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width:100%"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("background-color: white"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("All_form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_10 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onsubmit", new global::Microsoft.AspNetCore.Html.HtmlString("event.preventDefault(); ValidateTest();"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_11 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ShowRaysForDoctor", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_12 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("position: relative; left: 51%;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
  
    ViewData["Title"] = "Create";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("Styles", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3aa4a401334a18046de7a0de29c075db2bfc3d7e8630", async() => {
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
            WriteLiteral("<div style=\"width:1px;height:1px; overflow:scroll; visibility:hidden\">\r\n    <ul>\r\n");
#nullable restore
#line 12 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
          
            List<SelectListItem> RayTypes = ViewBag.Ray_Type_Id;
            for (int i = 0; i < RayTypes.Count; i++)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li class=\"Ray_Type_Id\">");
#nullable restore
#line 16 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
                                   Write(RayTypes[i].Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n                <li class=\"Ray_Type_Name\">");
#nullable restore
#line 17 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
                                     Write(RayTypes[i].Text);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n");
#nullable restore
#line 18 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
            }
        

#line default
#line hidden
#nullable disable
            WriteLiteral("    </ul>\r\n</div>\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3aa4a401334a18046de7a0de29c075db2bfc3d7e11206", async() => {
                WriteLiteral(@"
    <div class=""container"">
        <div class=""row"">
            <div class=""All_div col-12"" id=""div0"">
                <div class=""All_div col-12"">
                    <label for=""files"" class=""control-label""></label>
                    <input type=""file"" name=""files"" accept=""image/*"" required />
                </div>
                <div class=""All_div col-12"">
                    <label for=""ray"" class=""control-label""></label>
                    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3aa4a401334a18046de7a0de29c075db2bfc3d7e11956", async() => {
                    WriteLiteral("\r\n                    ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Name = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
#nullable restore
#line 33 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = ViewBag.Ray_Type_Id;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                </div>
                <div class=""All_div col-12"">
                    <label for=""date"" class=""control-label""></label>
                    <input type=""date"" name=""date"" class=""ValidateDates"" style=""width:100%"" /><br />
                    <span id=""DatesSpan0"" style=""color:red""></span>
                </div>
                <span class=""span"" id=""0""></span>
                <button type=""button"" onclick=""AddRay(this,0)"" style=""width: 100px; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600; ""> ?????????? ????????</button>
            </div>

            <input type=""hidden"" name=""medicalDetailId""");
                BeginWriteAttribute("value", " value=\"", 2052, "\"", 2084, 1);
#nullable restore
#line 45 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
WriteAttributeValue("", 2060, Model.Medical_Detail_Id, 2060, 24, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n            <input type=\"hidden\" name=\"DocId\"");
                BeginWriteAttribute("value", " value=\"", 2135, "\"", 2157, 1);
#nullable restore
#line 46 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
WriteAttributeValue("", 2143, ViewBag.DocId, 2143, 14, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n            <input type=\"hidden\" name=\"HoId\"");
                BeginWriteAttribute("value", " value=\"", 2207, "\"", 2228, 1);
#nullable restore
#line 47 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
WriteAttributeValue("", 2215, ViewBag.HoId, 2215, 13, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n            <div class=\"All_div col-12\">\r\n                <input type=\"submit\" value=\"?????????? ????????????\" style=\"width:100px\" class=\"btn btn-primary\" />\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_10);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3aa4a401334a18046de7a0de29c075db2bfc3d7e17702", async() => {
                WriteLiteral("???????? ??????????");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_11.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_11);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 56 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
                                        WriteLiteral(Model.Medical_Detail_Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 56 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
                                                                                   WriteLiteral(ViewBag.DocId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["DocId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-DocId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["DocId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 56 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
                                                                                                                   WriteLiteral(ViewBag.HoId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["HoId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-HoId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["HoId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_12);
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
        function ValidateTest() {
            var dates = document.getElementsByClassName(""ValidateDates"");
            var bool = true;
            for (var i = 0; i < dates.length; i++) {

                if (dates[i].value.substring(0, 4) != new Date().getFullYear()) {
                    document.getElementById('DatesSpan' + i).innerHTML = ""???? ???????? ?????????? ???????? ?????? ???? ?????? ??????????"";
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
#line 82 "C:\Users\Lenovo\Desktop\SHM\SHM\SHM(Smart Hospital Management)\Views\Medical_Ray\Create.cshtml"
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
                var Ray_Type_Ids = document.getElementsByClassName('Ray_Type_Id');
                var Ray_Type_Names = document.getElementsByClassName('Ray_Type_Name');
                var string1 = """";
                for (var j = 0; j < Ray_Type_Ids.length; j++) {
                    string1 += '<option value=""' + Ray_Type_Ids[j].innerHTML + '"">' + Ray_Type_Names[j].innerHTML + '</option>';
                }

                var valueToAdd = """";
                if (RaysCount != 2) {
                    valueToAdd = '<div class=""All_div col-12"" id=""div' + (i + 1) + '"">' +
                        '<div class=""All_div col-12"">' +
                        '<");
                WriteLiteral(@"label for= ""files"" class= ""control-label"" ></label> ' +
                        '<input type=""file"" name=""files"" accept=""image/*"" required />' +
                        '</div >' +
                        '<div class=""All_div col-12"">' +
                        '<label for=""ray"" class=""control-label""></label>' +
                        '<select name=""ray"" class=""form-control"">' +
                        string1 +
                        '</select>' +
                        '</div>' +
                        '<div class=""All_div col-12"">' +
                        '<label for=""date"" class=""control-label""></label>' +
                        '<input type=""date"" name=""date"" class=""ValidateDates"" required /> <br />' +
                        '<span id=""DatesSpan' + (i + 1) + '""  style=""color:red""></span>' +
                        '</div>' +
                        '<span class=""span"" id=""' + (i + 1) + '""></span>' +
                        '<button type=""button"" onclick=""AddRay(this,' + (i + 1) + '");
                WriteLiteral(@")"" style=""width: 100px; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600; ""> ?????????? </button>' +
                        '<button type=""button"" onclick=""DeleteRay(' + (i + 1) + ')"" style=""width: 100px; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600; ""> ?????? </button>' +
                        '</div>';
                }
                else {
                    valueToAdd = '<div class=""All_div col-12"" id=""div' + (i + 1) + '"">' +
                        '<div class=""All_div col-12"">' +
                        '<label for= ""files"" class= ""control-label"" ></label> ' +
                        '<input type=""file"" name=""files"" accept=""image/*"" required />' +
                        '</div >' +
                        '<div class=""All_div col-12"">' +
                        '<label for=""ray"" class=""control-label""></label>' +
                        '<select name=""ray"" class=""form");
                WriteLiteral(@"-control"">' +
                        string1 +
                        '</select>' +
                        '</div>' +
                        '<div class=""All_div col-12"">' +
                        '<label for=""date"" class=""control-label""></label>' +
                        '<input type=""date"" name=""date"" class=""ValidateDates"" required /> <br />' +
                        '<span id=""DatesSpan' + (i + 1) + '""  style=""color:red""></span>' +
                        '</div>' +
                        '<span class=""span"" id=""' + (i + 1) + '""></span>' +
                        '<button type=""button"" onclick=""DeleteRay(' + (i + 1) + ')"" style=""width: 100px; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600; ""> ?????? </button>' +
                        '</div>';
                }
                div.insertAdjacentHTML('afterend', valueToAdd);
                RaysCount++;
            }
        }
        function DeleteRay(i) {
           ");
                WriteLiteral(@" var div = document.getElementById(""div"" + i);
            div.parentNode.removeChild(div);
            RaysCount--;
            var spans = document.getElementsByClassName('span');
            if (RaysCount == 1) {
                var span = document.getElementById('0');
                span.insertAdjacentHTML('afterend', '<button type=""button"" onclick=""AddRay(this,0)"" style=""width: 100px; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600; ""> ?????????? ???????? </button>');
            }
            else {
                spans[spans.length - 1].insertAdjacentHTML('afterend', '<button type=""button"" onclick=""AddRay(this,' + spans[spans.length - 1].id + ')"" style=""width: 100px; height: 35px; border-radius: 16px; border: none; background-color: #bac8d5e0; color: #5e5c5c; font-weight: 600; ""> ?????????? </button>');
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables.Medical_Ray> Html { get; private set; }
    }
}
#pragma warning restore 1591
