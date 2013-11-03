
if (typeof(avt) == 'undefined') { avt = {} }

avt.ht = {

    $: avt_jQuery_1_3_2_ht1,

    init: function(cfg) {
        avt.ht.$(document).ready(function() {
            avt.ht.initThumbnails(cfg);
        });

        function EndRequestHandler(sender, args) {
            // js code here; runs only on async postbacks
            avt.ht.initThumbnails(cfg);
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    },
    
    initThumbnails: function(cfg) {
        if (cfg.opts.autogeneratefromlabels)
                avt.ht.autoGenerateFromLabels(cfg);
            avt.ht.generateFromItems(cfg);
            
            avt.ht.$("form")[0].onsubmit = function() {
                //alert("submit");
            };
    },
    
    autoGenerateFromLabels: function(cfg) {

        avt.ht.$("label a img[alt]").each(function() {

            if (this.getAttribute("alt").length == 0)
                return;

            var _img = avt.ht.$(this);                
            var _label = _img.parents("label:first");
            var bMatch = false;

            if (_label.attr("for") != null && _label.attr("for").length > 0 && avt.ht.$("#" + _label.attr("for")).size() > 0) { // simple case, there is a for attribute on the label making the connection to the control
                
                var _input = avt.ht.$("#" + _label.attr("for"));
                if (_input.size() > 0) {
                    var item = avt.ht.$.extend(true, {obj: _input, text: _img.attr("alt")}, cfg.opts); 
                    avt.ht.generateTooltip(item);
                    bMatch = true;
                } else {
                    // match child controls
                    _input = avt.ht.$("[id^='" + _label.attr("for") + "']:input");
                    if (_input.size() > 0) {
                        _input.each(function() {
                            var item = avt.ht.$.extend(true, {obj: this, text: _img.attr("alt")}, cfg.opts); 
                            avt.ht.generateTooltip(item);
                            bMatch = true;
                        });
                    } else {
                        // also try name, make sure to replace _ with $
                        _input = avt.ht.$("[name^='" + _label.attr("for").replace(/_/g, "$") + "']:input");

                        if (_input.size() > 0) {
                            _input.each(function() {
                                var item = avt.ht.$.extend(true, {obj: this, text: _img.attr("alt")}, cfg.opts); 
                                avt.ht.generateTooltip(item);
                                bMatch = true;
                            });
                        }
                    }
                    
                }

            } else {
                // see if the label is inside a td, most likely next td will contain the input fields
                if (_label.parent()[0].nodeName.toLowerCase() == "td" && _label.parent().next("td").length > 0) {
                
                    // match child controls
                    _input = _label.parent().next("td").find(":input");
                    if (_input.length == 0)
                        _input = _label.parent().next("td").children();
                        
                    _input.each(function() {
                        var item = avt.ht.$.extend(true, {obj: this, text: _img.attr("alt")}, cfg.opts); 
                        avt.ht.generateTooltip(item);
                        bMatch = true;
                    });
                } else if (_label.parent().parent()[0].nodeName.toLowerCase() == "div" && _label.parent().parent().next("div").length > 0 && _label.parent().parent().css("float") == "left") {
                
                    // match child controls
                    _input = _label.parent().parent().next("div").find(":input");
                    if (_input.length == 0)
                        _input = _label.parent().parent().next("div").children();

                    _input.each(function() {
                        var item = avt.ht.$.extend(true, {obj: this, text: _img.attr("alt")}, cfg.opts); 
                        avt.ht.generateTooltip(item);
                        bMatch = true;
                    });
                }
            }

            if (bMatch && cfg.opts.autogeneratehidednnlabels)
                _img.hide();

        });
        
        // do IndooGrid
        avt.ht.$("td.ge_label").parent().each(function() {
            var _text = avt.ht.$.trim(avt.ht.$(this).find(".ge_help:first").find(".Help").html());
            if (_text.length > 0) {
            
                // we have help string, generate tooltips for all inputs in this field
                if (avt.ht.$(this).find(":input").size() > 0) {
                    avt.ht.$(this).find(":input").each(function() {
                        var item = avt.ht.$.extend(true, {obj: this, text: _text}, cfg.opts); 
                        avt.ht.generateTooltip(item);
                    });
                } else {
                    avt.ht.$(this).next().find(":input").each(function() {
                        var item = avt.ht.$.extend(true, {obj: this, text: _text}, cfg.opts); 
                        avt.ht.generateTooltip(item);
                    });
                }
                
                // hide the original help label
                if (cfg.opts.autogeneratehidednnlabels) {
                    avt.ht.$(this).find(".ge_help img").hide();
                }
            }
        });
    },
    
    generateFromItems: function(cfg) {
        for (i = 0; i < cfg.items.length; i++) {
            var styles = cfg.items[i].textcssstyle;
            cfg.items[i] = avt.ht.$.extend(true, {}, cfg.opts, cfg.items[i]);
            
            // concatenate styles, item styles take priority
            if (typeof(styles) != "undefined" && typeof(cfg.opts.textcssstyle) != "undefined") {
                cfg.items[i].textcssstyle = cfg.opts.textcssstyle + styles;
            }
            
            if (!isNaN(cfg.items[i].width)) {
                cfg.items[i].width = "" + cfg.items[i].width + "px";
            }
            
            avt.ht.$(cfg.items[i].selector).each(function() {
                avt.ht.generateTooltip(avt.ht.$.extend(true, {obj: this}, cfg.items[i]));
            });
        }
    },
    
    generateTooltip: function(item) {
        //alert(avt.ht.$("<div style = \"" + item.textcssstyle + "; zIndex: 99999; \"></div>")[0].style);
        /* var style = avt.ht.$("<div style = \"" + item.textcssstyle + "; zIndex: 99999; \"></div>")[0].style;
        for (var s in style)
            alert(s + ":" + style[s]); */

        item.obj = avt.ht.$(item.obj);
        item.obj
        .attr("title", item.text)
        .attr("bt-xtitle", item.text)
        .bt({
            padding: item.padding,
            width: item.width,
            spikeLength: item.spikelength,
            spikeGirth: item.spikewidth,
            cornerRadius: item.cornerradius,
            fill: item.fill,
            strokeWidth: item.strokewidth,
            strokeStyle: item.strokcolor,
            cssStyles: item.textcssstyle + "; z-index: 99999", //{color: '#FFF', fontWeight: 'bold', zIndex: '99999'},
            cssClass: item.textcssclass,
            offsetParent: avt.ht.$("body"),
            trigger: avt.ht.determineTrigger(item),
            closeWhenOthersOpen: !item.showalways,
            clickAnywhereToClose: !item.showalways,
            positions: item.positions,
            centerPointX: item.centerpointx,
            centerPointY: item.centerpointy,
            offsetY: item.offsety,
            offsetX: item.offsetx,
            shadow: item.drawshadow,
            showTip: avt.ht.getShowFn(item),
            hideTip: avt.ht.getHideFn(item)
        });
    },
    
    determineTrigger: function(item) {
    
        if (item.showalways) return 'now';
        if (item.triggeron == "focus") return ['focus', 'blur'];
        if (item.triggeron == "hover") return "hover";
        
        if (item.triggeron == "auto") {
            var trigger = "hover";
            if (item.obj.attr("type") != null && item.obj.attr("type").toLowerCase() == "text") return ['focus mouseover', 'blur'];
            if (item.obj[0].nodeName.toLowerCase() == "textarea") return ['focus', 'blur'];
            return "hover";
        }
        
        // return whatever is inside there, may be a more advanced syntax or may be illegal syntax
        if (item.triggeron[0] == "[")
            item.triggeron = eval(item.triggeron);
        return item.triggeron;
    },
    
    getShowFn: function(item) {
        
        var fn = function(box) {
            avt.ht.$(box).show();
        };
        
        switch (item.effectonshow) {
            case "fadeIn":
                fn = function(box) {
                    //avt.ht.$(box).show();//.animate({opacity: 1});
                    avt.ht.$(box).fadeIn(item.effectonshowms);
                };
                break;
            case "scaleUp":
                fn = function(box){
                    var $content = avt.ht.$('.bt-content', box).hide(); /* hide the content until after the animation */
                    var $canvas = avt.ht.$('canvas', box).hide(); /* hide the canvas for a moment */
                    var origWidth = $canvas[0].width; /* jQuery's .width() doesn't work on canvas element */
                    var origHeight = $canvas[0].height;
                    avt.ht.$(box).show(); /* show the wrapper, however elements inside (canvas, content) are now hidden */
                    $canvas
                        .css({width: origWidth * .5, height: origHeight * .5, left: origWidth * .25, top: origHeight * .25, opacity: .1})
                        .show()
                        .animate({width: origWidth, height: origHeight, left: 0, top: 0, opacity: 1}, item.effectonshowms, '',
                            function(){$content.show()} /* show the content when animation is done */
                        );
                }
                break;
            case "scaleUpBounce":
                fn = function(box){
                    var $content = avt.ht.$('.bt-content', box).hide(); /* hide the content until after the animation */
                    var $canvas = avt.ht.$('canvas', box).hide(); /* hide the canvas for a moment */
                    var origWidth = $canvas[0].width; /* jQuery's .width() doesn't work on canvas element */
                    var origHeight = $canvas[0].height;
                    avt.ht.$(box).show(); /* show the wrapper, however elements inside (canvas, content) are now hidden */
                    $canvas
                        .css({width: origWidth * .5, height: origHeight * .5, left: origWidth * .25, top: origHeight * .25, opacity: .1})
                        .show()
                        .animate({width: origWidth, height: origHeight, left: 0, top: 0, opacity: 1}, item.effectonshowms, 'easeOutBounce',
                            function(){$content.show()} /* show the content when animation is done */
                        );
                }
                break;
        }
        
        return fn;
    },
    
    getHideFn: function(item) {
        
        var fn = function(box, callback) {
            try { avt.ht.$(box).hide(); } catch (e) {}
            callback();   // you MUST call "callback" at the end of your animations
        };
        
        switch (item.effectonhide) {
            case "fadeOut":
                fn = function(box, callback) {
                    try { avt.ht.$(box).animate({opacity: 0},item.effectonhidems, callback); } catch (e) {}
                    //callback();   // you MUST call "callback" at the end of your animations
                };
                break;
            case "scaleDown":
                fn = function(box, callback) {
                    var $content = avt.ht.$('.bt-content', box).hide();
                    var $canvas = avt.ht.$('canvas', box);
                    var origWidth = $canvas[0].width;
                    var origHeight = $canvas[0].height;
                    try {
                        $canvas.animate({width: origWidth * .5, height: origHeight * .5, left: origWidth * .25, top: origHeight * .25, opacity: 0}, item.effectonhidems, 'swing', callback); /* callback */
                    } catch (e) {}
                };
                break;
        }
        
        return fn;
    }

}