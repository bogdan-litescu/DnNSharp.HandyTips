using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Framework;
using System.Web.UI.HtmlControls;
using System.Text;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security;

namespace avt.HandyTips
{
    public enum eTriggerOnAction
    {
        Auto,
        Hover,
        Focus
    }

    public enum ePosition
    {
        most,
        top,
        bottom,
        left,
        right
    }

    public enum eEffectShow
    {
        noEffect,
        fadeIn,
        scaleUp,
        scaleUpBounce
    }

    public enum eEffectHide
    {
        noEffect,
        fadeOut,
        scaleDown
    }

    //public enum eAutoGenerateTooltipLocation
    //{
    //    Controls,
    //    LabelText,
    //    ControlsAndLabelText
    //}

    public partial class HandyTips : System.Web.UI.UserControl
    {
        private bool _AutoGenerateFromLabels = true;
        public bool AutoGenerateFromLabels { get { return _AutoGenerateFromLabels; } set { _AutoGenerateFromLabels = value; } }

        private bool _AutoGenerateHideDnnLabels = true;
        public bool AutoGenerateHideDnnLabels { get { return _AutoGenerateHideDnnLabels; } set { _AutoGenerateHideDnnLabels = value; } }

        //private eAutoGenerateTooltipLocation _AutoGenerateTooltipsOn = eAutoGenerateTooltipLocation.Controls;
        //public eAutoGenerateTooltipLocation AutoGenerateTooltipsOn { get { return _AutoGenerateTooltipsOn; } set { _AutoGenerateTooltipsOn = value; } }

        private eTriggerOnAction _TriggerOn = eTriggerOnAction.Auto;
        public eTriggerOnAction TriggerOn { get { return _TriggerOn; } set { _TriggerOn = value; } }

        private bool _ShowAlways = false;
        public bool ShowAlways { get { return _ShowAlways; } set { _ShowAlways = value; } }

        private int _SpikeWidth = 10;
        public int SpikeWidth { get { return _SpikeWidth; } set { _SpikeWidth = value; } }

        private int _SpikeLength = 15;
        public int SpikeLength { get { return _SpikeLength; } set { _SpikeLength = value; } }

        private int _Width = 200;
        public int Width { get { return _Width; } set { _Width = value; } }

        private int _Padding = 20;
        public int Padding { get { return _Padding; } set { _Padding = value; } }

        private int _CornerRadius = 20;
        public int CornerRadius { get { return _CornerRadius; } set { _CornerRadius = value; } }

        private string _Fill = "rgba(FF, 0, 0, .8)";
        public string Fill { get { return _Fill; } set { _Fill = value; } }

        private int _StrokeWidth = 2;
        public int StrokeWidth { get { return _StrokeWidth; } set { _StrokeWidth = value; } }

        private string _StrokColor = "#CCC000";
        public string StrokColor { get { return _StrokColor; } set { _StrokColor = value; } }

        private string _TextCssStyle = "color: #FFFFFF; font-weight: bold;";
        public string TextCssStyle { get { return _TextCssStyle; } set { _TextCssStyle = value; } }

        private string _TextCssClass = "";
        public string TextCssClass { get { return _TextCssClass; } set { _TextCssClass = value; } }

        private double _CenterPointX = 0.5;
        public double CenterPointX { get { return _CenterPointX; } set { _CenterPointX = value; } }

        private double _CenterPointY = 0.5;
        public double CenterPointY { get { return _CenterPointY; } set { _CenterPointY = value; } }

        private ePosition _Position = ePosition.most;
        public ePosition Position { get { return _Position; } set { _Position = value; } }

        private int _OffsetX = 0;
        public int OffsetX { get { return _OffsetX; } set { _OffsetX = value; } }

        private int _OffsetY = 0;
        public int OffsetY { get { return _OffsetY; } set { _OffsetY = value; } }

        private bool _DrawShadow = false;
        public bool DrawShadow { get { return _DrawShadow; } set { _DrawShadow = value; } }

        private eEffectShow _EffectOnShow = eEffectShow.noEffect;
        public eEffectShow EffectOnShow { get { return _EffectOnShow; } set { _EffectOnShow = value; } }

        private int _EffectOnShowSpeedMs = 500;
        public int EffectOnShowSpeedMs { get { return _EffectOnShowSpeedMs; } set { _EffectOnShowSpeedMs = value; } }

        private int _MsDelayShow = 0;
        public int MsDelayShow { get { return _MsDelayShow; } set { _MsDelayShow = value; } }

        private eEffectHide _EffectOnHide = eEffectHide.noEffect;
        public eEffectHide EffectOnHide { get { return _EffectOnHide; } set { _EffectOnHide = value; } }

        private int _EffectOnHideSpeedMs = 500;
        public int EffectOnHideSpeedMs { get { return _EffectOnHideSpeedMs; } set { _EffectOnHideSpeedMs = value; } }

        private int _MsDelayHide = 0;
        public int MsDelayHide { get { return _MsDelayHide; } set { _MsDelayHide = value; } }

        //private Style _TextStyle = new Style();
        //public Style TextStyle { get { return _TextStyle; } set { _TextStyle = value; } }

        //private Style _Items = null;

        //public Style Items
        //{
        //    get { return _Items; }
        //    set { _Items = value; }
        //}

        //private ITemplate _Items;

        //[TemplateContainer(typeof(LayoutContainer))]
        //public ITemplate Items
        //{
        //    get { return _Items; }
        //    set { _Items = value; }
        //}

        List<HtmlGenericControl> _Items = new List<HtmlGenericControl>();
        public List<HtmlGenericControl> Items { get { return _Items; } }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            //Items.InstantiateIn(lblTest);

            AJAX.RegisterScriptManager();

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("avt_jQuery_1_3_2_ht1")) {
                Page.ClientScript.RegisterClientScriptInclude("avt_jQuery_1_3_2_ht1", TemplateSourceDirectory + "/js/jquery-1.3.2.av3.js");
            }

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("avt_jQueryUi_1_7_2_eff_ht1")) {
                Page.ClientScript.RegisterClientScriptInclude("avt_jQueryUi_1_7_2_eff_ht1", TemplateSourceDirectory + "/js/jquery-ui-1.7.2.avt-1.5.eff.js");
            }

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("jQuery.BeautyTips.0.9.5")) {
                Page.ClientScript.RegisterClientScriptInclude("jQuery.BeautyTips.0.9.5", TemplateSourceDirectory + "/js/beautytips/bt-0.9.5-rc1/jquery.bt.js");
            }

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("excanvas_r3")) {
                Page.ClientScript.RegisterClientScriptInclude("excanvas_r3", TemplateSourceDirectory + "/js/excanvas_r3/excanvas.js");
            }

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("avt_HandyTips_1_4")) {
                Page.ClientScript.RegisterClientScriptInclude("avt_HandyTips_1_4", TemplateSourceDirectory + "/js/avt.HandyTips-1.4.js");
            }

            if (Page.Header.FindControl("theme.jquery.ui") == null) {
                HtmlLink htmlLink = new HtmlLink();
                htmlLink.Href = TemplateSourceDirectory + "/js/beautytips/bt-0.9.5-rc1/jquery.bt.css";
                htmlLink.Attributes["type"] = "text/css";
                htmlLink.Attributes["rel"] = "stylesheet";
                htmlLink.ID = "theme.jquery.ui";
                Page.Header.Controls.Add(htmlLink);
            }

            // init
            // TODO: js encode strings
            Page.ClientScript.RegisterStartupScript(this.GetType(), "avt.HandyTips.init", "avt.ht.$(document).ready(function() { avt.ht.init(" + BuildJson() + "); });", true);
        }


        private string BuildJson()
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("{opts:");
            sbJson.Append(BuildOptionsJson());
            sbJson.Append(",items:[");
            sbJson.Append(BuildItemsJson());
            sbJson.Append("]}");
            return sbJson.ToString();
        }

        private string BuildOptionsJson()
        {
            StringBuilder sbJson = new StringBuilder();

            sbJson.Append("{");
            sbJson.Append("autogeneratefromlabels:" + (AutoGenerateFromLabels ? "true" : "false") + ",");
            sbJson.Append("autogeneratehidednnlabels:" + (AutoGenerateHideDnnLabels ? "true" : "false") + ",");
            //sbJson.Append("autogeneratetooltipson:'" + AutoGenerateTooltipsOn.ToString().ToLower() + "',");
            
            sbJson.Append("triggeron:'" + TriggerOn.ToString().ToLower() + "',");
            sbJson.Append("showalways:" + (ShowAlways ? "true" : "false") + ",");
            sbJson.Append("spikewidth:" + SpikeWidth.ToString() + ",");
            sbJson.Append("spikelength:" + SpikeLength.ToString() + ",");
            sbJson.Append("width:'" + Width.ToString() + "px',");
            sbJson.Append("padding:" + Padding.ToString() + ",");
            sbJson.Append("cornerradius:" + CornerRadius.ToString() + ",");
            sbJson.Append("fill:'" + Fill + "',");
            sbJson.Append("strokewidth:" + StrokeWidth.ToString() + ",");
            sbJson.Append("strokcolor:'" + StrokColor+"',");
            sbJson.Append("textcssstyle:'" + TextCssStyle + "',");
            sbJson.Append("textcssclass:'" + TextCssClass + "',");
            sbJson.Append("centerpointx:" + CenterPointX.ToString().Replace(',', '.') + ",");
            sbJson.Append("centerpointy:" + CenterPointY.ToString().Replace(',', '.') + ",");
            sbJson.Append("positions:['" + Position.ToString() + "'],");
            sbJson.Append("offsetx:" + OffsetX + ",");
            sbJson.Append("offsety:" + OffsetY + ",");
            sbJson.Append("drawshadow:" + (DrawShadow ? "true" : "false") + ",");
            sbJson.Append("effectonshow:'" + EffectOnShow.ToString() + "',");
            sbJson.Append("effectonshowms:" + EffectOnShowSpeedMs.ToString() + ",");
            sbJson.Append("msdelayshow:" + MsDelayShow.ToString() + ",");
            sbJson.Append("effectonhide:'" + EffectOnHide.ToString() + "',");
            sbJson.Append("effectonhidems:" + EffectOnHideSpeedMs.ToString() + ",");
            sbJson.Append("msdelayhide:" + MsDelayHide.ToString() + "");
            sbJson.Append("}");

            return sbJson.ToString();
        }


        private string BuildItemsJson()
        {
            StringBuilder sbJson = new StringBuilder();

            string culture = "";
            try {
                culture = ((PageBase)Page).PageCulture.Name;
            } catch { culture = ""; }
            
           // //DotNetNuke.Services.Localization.Localization.GetEnabledLocales

            foreach (HtmlGenericControl ctrl in Items) {
                if (!string.IsNullOrEmpty(ctrl.Attributes["lang"]) && !string.IsNullOrEmpty(culture)) {
                    if (ctrl.Attributes["lang"] != culture) {
                        continue; // wrong locale
                    }
                }

                StringBuilder sbItem = new StringBuilder();
                sbItem.Append("{");
                foreach (string attrName in ctrl.Attributes.Keys) {
                    sbItem.Append(attrName.ToLower() + ":'" + ctrl.Attributes[attrName] + "',");
                }

                if (!string.IsNullOrEmpty(ctrl.InnerHtml)) {
                    sbItem.Append("text:'" + JsonEncode(ctrl.InnerHtml.Trim()) + "',");
                }

                if (sbItem[sbItem.Length - 1] == ',')
                    sbItem.Remove(sbItem.Length - 1, 1);

                if (sbItem.Length > 1) { // check not empty item
                    sbItem.Append("},");
                    sbJson.Append(sbItem.ToString());
                }
            }

            if (sbJson.Length > 0 && sbJson[sbJson.Length - 1] == ',')
                sbJson.Remove(sbJson.Length - 1, 1);

            return sbJson.ToString();
        }


        private static string JsonEncode(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";

            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            //sb.Append('"');
            for (i = 0; i < len; i += 1) {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>') || (c == '\'')) {
                    sb.Append('\\');
                    sb.Append(c);
                } else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else {
                    if (c < ' ') {
                        //t = "000" + Integer.toHexString(c); 
                        string tmp = new string(c, 1);
                        t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    } else {
                        sb.Append(c);
                    }
                }
            }
            //sb.Append('"');
            return sb.ToString();
        }
    }
}
