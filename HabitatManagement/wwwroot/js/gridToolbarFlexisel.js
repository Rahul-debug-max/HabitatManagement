function setMMSHomeMenuFlexi() {
    var homeMenuMMS = $("#flexibleCrouserMMS");

    homeMenuMMS.owlCarousel({

        itemsCustom: [
          [0, 2],
          [300, 3],
          [450, 4],
          [600, 5],
          [700, 6],
          [850, 7],
          [1000, 8],
          [1200, 11],
          [1400, 13],
          [1600, 15]
        ],
        navigation: true
    });
}

function setHomeMenuFlexi() {
    var homeMenu = $("#flexibleCrouser");

    homeMenu.owlCarousel({

        itemsCustom: [
          [0, 2],
          [300, 3],
          [450, 4],
          [600, 5],
          [700, 6],
          [850, 7],
          [1000, 8],
          [1200, 11],
          [1400, 13],
          [1600, 15]
        ],
        navigation: true

    });
}
//function setEWOGridToolBarFlexi(languageCode) {
//    if (languageCode == "en-GB" || languageCode == "fr-FR") {
//        var ewoGrid = $("#ewoGridToolbarFlexi");

//        ewoGrid.owlCarousel({

//            itemsCustom: [
//              [0, 3],
//              [300, 4],
//              [450, 6],
//              [600, 7],
//              [700, 9],
//              [850, 9],
//              [1000, 9],
//              [1200, 9],
//              [1400, 9],
//              [1600, 9]
//            ],
//            navigation: true

//        });
//    }
//    else if (languageCode == "sv-SE") {
//    var ewoGrid = $("#ewoGridToolbarFlexi");

//    ewoGrid.owlCarousel({

//        itemsCustom: [
//              [0, 2],
//              [300, 3],
//              [450, 5],
//              [600, 5],
//              [700, 6],
//              [850, 7],
//              [1000, 7],
//              [1200, 7],
//              [1300, 7],
//              [1350, 8],
//              [1400, 9],
//              [1600, 9]
//            ],
//            navigation: true

//        });
//    }
//    else {
//        var ewoGrid = $("#ewoGridToolbarFlexi");

//        ewoGrid.owlCarousel({

//            itemsCustom: [
//          [0, 3],
//          [300, 4],
//          [450, 6],
//          [600, 7],
//          [700, 9],
//          [850, 9],
//          [1000, 9],
//          [1200, 9],
//          [1400, 9],
//          [1600, 9]
//        ],
//        navigation: true

//    });
//    }
//}
//function setWorkOrderGridToolBarFlexi(languageCode) {
//    if (languageCode == "en-GB" || languageCode == "sv-SE" || languageCode == "fr-FR") {
//        var workOrderGrid = $("#workorderGridToolbarFlexi");

//        workOrderGrid.owlCarousel({

//            itemsCustom: [
//               [0, 3],
//              [350, 4],
//              [400, 5],
//              [490, 6],
//              [600, 7],
//              [700, 7],
//              [850, 7],
//              [1000, 7],
//              [1200, 7],
//              [1400, 7],
//              [1600, 7]
//            ],
//            navigation: true

//        });
//    }
    
//    else {
//    var workOrderGrid = $("#workorderGridToolbarFlexi");

//    workOrderGrid.owlCarousel({

//        itemsCustom: [
//           [0, 3],
//          [350, 4],
//          [400, 5],
//          [490, 6],
//          [600, 7],
//          [700, 7],
//          [850, 7],
//          [1000, 7],
//          [1200, 7],
//          [1400, 7],
//          [1600, 7]
//        ],
//        navigation: true

//    });
//    }
//}
//function setWOListGridToolBarFlexi(languageCode) {
//    if (languageCode == "en-GB") {
//    var woListGrid = $("#wolistGridToolbarFlexi");

//    woListGrid.owlCarousel({

//        itemsCustom: [
//          [0, 3],
//          [300, 4],
//          [450, 6],
//          [600, 7],
//          [745, 8],
//          [850, 11],
//          [1000, 11],
//          [1200, 13],
//          [1400, 13],
//          [1600, 12]
//        ],
//        navigation: true

//    });
//    }
//    else if (languageCode == "sv-SE" || languageCode == "fr-FR") {
//        var woListGrid = $("#wolistGridToolbarFlexi");

//        woListGrid.owlCarousel({

//        itemsCustom: [
//           [0, 3],
//              [300, 4],
//              [450, 6],
//              [600, 7],
//              [745, 8],
//              [850, 10],
//              [1000, 11],
//              [1200, 11],
//              [1400, 13],
//              [1600, 12]
//            ],
//            navigation: true

//        });
//    }
//    else {
//        var woListGrid = $("#wolistGridToolbarFlexi");

//        woListGrid.owlCarousel({

//            itemsCustom: [
//              [0, 3],
//              [300, 4],
//              [450, 6],
//              [600, 7],
//              [745, 8],
//              [850, 11],
//              [1000, 11],
//              [1200, 13],
//              [1400, 13],
//              [1600, 12]
//            ],
//            navigation: true

//        });
//    }
//}
//function setJobMonitorGridToolBarFlexi(languageCode) {
//    if (languageCode == "en-GB" || languageCode == "fr-FR") {
//        var listGridJob = $("#jobListGrid");

//        listGridJob.owlCarousel({

//            itemsCustom: [
//              [0, 4],
//              [400, 6],
//              [550, 7],
//              [650, 8],
//              [745, 9],
//              [850, 9],
//              //[900,9],
//              [960, 8],
//              [1000, 8],
//              //[1050, 8],
//              [1100, 9],
//              [1200, 10],
//              [1400, 11],
//              [1600, 11]
//            ],
//            navigation: true

//        });
//    }
//    else if (languageCode == "sv-SE") {
//        var listGridJob = $("#jobListGrid");

//        listGridJob.owlCarousel({

//            itemsCustom: [
//              [0, 4],
//              [400, 5],
//              [550, 6],
//              [650, 7],
//              [745, 8],
//              [850,9],
//              [950, 9],
//              [1000, 8],
//              [1100, 9],
//              [1200, 10],
//              [1400, 13],
//              [1600, 13]
//        ],
//        navigation: true

//    });
//    }
//    else {
//    var listGridJob = $("#jobListGrid");

//    listGridJob.owlCarousel({

//        itemsCustom: [
//          [0, 4],
//          [400, 6],
//          [550, 7],
//          [650, 8],
//          [745, 9],
//          [850, 11],
//          [1000, 13],
//          [1200, 13],
//          [1400, 13],
//          [1600, 13]
//        ],
//        navigation: true

//    });
//    }
//}
//function setTagListGridToolBarFlexi(languageCode) {
//    if (languageCode == "en-GB" || languageCode == "fr-FR") {
//        var tagListGrid = $("#taglisGridToolbarFlexi");

//        tagListGrid.owlCarousel({

//            itemsCustom: [
//               [0, 3],
//              [350, 4],
//              [400, 5],
//              [490, 5],
//              [600, 5],
//              [700, 5],
//              [850, 5],
//              [1000, 5],
//              [1200, 5],
//              [1400, 5],
//              [1600, 5]
//            ],
//            navigation: true

//        });
//    }

//    else if (languageCode == "sv-SE") {
//        var tagListGrid = $("#taglisGridToolbarFlexi");

//        tagListGrid.owlCarousel({

//            itemsCustom: [
//               [0, 2],
//              [350, 3],
//              [500, 4],
//              [550, 4],
//              [600, 5],
//              [700, 5],
//              [850, 5],
//              [1000, 5],
//              [1200, 5],
//              [1400, 5],
//              [1600, 5]
//            ],
//            navigation: true

//        });
//    }
//    else {
//        var tagListGrid = $("#taglisGridToolbarFlexi");

//        tagListGrid.owlCarousel({

//            itemsCustom: [
//               [0, 3],
//              [350, 4],
//              [400, 5],
//              [490, 5],
//              [600, 5],
//              [700, 5],
//              [850, 5],
//              [1000, 5],
//              [1200, 5],
//              [1400, 5],
//              [1600, 5]
//            ],
//            navigation: true

//});
//    }
//}



function InitiliazeToolbarScrollBar(gridToolbarId) {
    if ($("#" + gridToolbarId).length > 0) {
        $("#" + gridToolbarId).siblings(".prevBtn").bind('click', function () {
            if ($("#" + gridToolbarId + " li").first()[0].getBoundingClientRect().left < $(this)[0].getBoundingClientRect().right ) {
                if ($("#" + gridToolbarId + " li.toolbarlihide").length > 0) {
                    $("#" + gridToolbarId + " li.toolbarlihide").last().removeClass("toolbarlihide");
                }
            }
        });
        $("#" + gridToolbarId).siblings(".nextBtn").bind('click', function () {
            if ($("#" + gridToolbarId + " li").not(":hidden").last()[0].getBoundingClientRect().right > $(this)[0].getBoundingClientRect().left) {
                if ($("#" + gridToolbarId + " li").not(".toolbarlihide").length > 0) {
                    $("#" + gridToolbarId + " li").not(".toolbarlihide").first().addClass("toolbarlihide");
                }
            }
        });
       var toolbarHeight = $("#" + gridToolbarId).parent("div").height();
        resetToolbarScrollBarButton(gridToolbarId);
        $(window).resize(function (e) {             
            resetToolbarScrollBarButton(gridToolbarId);
        });
    }
}

function resetToolbarScrollBarButton(gridToolbarId) {

    var toolbarHeight = $("#" + gridToolbarId).parent("div").height();
    if (toolbarHeight == 142 || toolbarHeight == 81) {
        //$(".spanBgGrey").css("height", "84");
        //$(".nextBtn").css("height", "84");
        $(".nextBtn .glyphicon-chevron-right").css("margin-top", "36px");
        $(".prevBtn .glyphicon-chevron-left").css({ "position": "relative", "top": "-4px" });
       // $(".prevBtn").css("height", "84");
    }
    else if (toolbarHeight == 56) {
       // $(".spanBgGrey").css("height", "59");
       // $(".nextBtn").css("height", "59");
        $(".nextBtn .glyphicon-chevron-right").css("margin-top", "22px");
        $(".prevBtn .glyphicon-chevron-left").css({ "position": "relative", "top": "22px" });
        //$(".prevBtn").css({ "height": "59", "padding": "0", "top": "11px" });
        $(".prevBtn").css({ "padding": "0", "top": "11px" });
    }
    
    if ($("#" + gridToolbarId).length > 0) {

        // 86334 - MVC Upgrade - RSK time part b - MVC upgrade functionality issue fixed
        if ($("#IsDisplayIconText") != undefined && $("#IsDisplayIconText").val() == "False") {
            $(".spanBgGrey").css("height", "54px");
            $(".nextBtn .glyphicon-chevron-right").css("margin-top", "20px");
            $(".prevBtn").css({ "padding": "0px", "height": "54px" });
            $(".prevBtn .glyphicon-chevron-left").css("margin-top", "20px");
        }

        $("#" + gridToolbarId + " li.toolbarlihide").removeClass("toolbarlihide");
        $("#" + gridToolbarId).siblings(".nextBtn").show();
        $("#" + gridToolbarId).siblings(".prevBtn").show();

        if ($("#" + gridToolbarId + " li").length > 0) {
            if (($("#" + gridToolbarId + " li").not(":hidden").last()[0].getBoundingClientRect().right < $("#" + gridToolbarId).siblings(".nextBtn")[0].getBoundingClientRect().left - 32)
                && ($("#" + gridToolbarId + " li").first()[0].getBoundingClientRect().left > $("#" + gridToolbarId).siblings(".prevBtn")[0].getBoundingClientRect().right)
            ) {

                $("#" + gridToolbarId).siblings(".nextBtn").hide();
                $("#" + gridToolbarId).siblings(".prevBtn").hide();
                $("#" + gridToolbarId).css({ "width": "auto", "text-align": "center" });
                $("#" + gridToolbarId).removeClass("ulPaddingToolbar");

                //var currenttoolbarHeight = $("#" + gridToolbarId).parent("div").height();

                var lastliheight = $("#" + gridToolbarId + " li").not(":hidden").last().position().top - $("#" + gridToolbarId).position().top;

                if (lastliheight > 20) {

                    $("#" + gridToolbarId).css({ "width": "2000px", "text-align": "left" });
                    $("#" + gridToolbarId).addClass("ulPaddingToolbar");
                    $("#" + gridToolbarId).siblings(".nextBtn").show();
                    $("#" + gridToolbarId).siblings(".prevBtn").show();
                }
                else {
                    $("#" + gridToolbarId).siblings(".nextBtn").hide();
                    $("#" + gridToolbarId).siblings(".prevBtn").hide();
                    $("#" + gridToolbarId).css({ "width": "auto", "text-align": "center" });
                    $("#" + gridToolbarId).removeClass("ulPaddingToolbar");
                }
            }
            else {
                $("#" + gridToolbarId).css({ "width": "2000px", "text-align": "left" });
                $("#" + gridToolbarId).addClass("ulPaddingToolbar");
                if (($("#" + gridToolbarId + " li").not(":hidden").last()[0].getBoundingClientRect().right < $("#" + gridToolbarId).siblings(".nextBtn")[0].getBoundingClientRect().left)) {
                    $("#" + gridToolbarId).siblings(".nextBtn").hide();
                    $("#" + gridToolbarId).siblings(".prevBtn").hide();
                    $("#" + gridToolbarId).css({ "width": "auto", "text-align": "center" });
                    $("#" + gridToolbarId).removeClass("ulPaddingToolbar");
                }
                else {

                    $("#" + gridToolbarId).siblings(".nextBtn").show();
                    $("#" + gridToolbarId).siblings(".prevBtn").show();
                }
            }
        }
    }
}