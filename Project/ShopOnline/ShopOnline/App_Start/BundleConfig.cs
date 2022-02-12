using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ShopOnline.App_Start
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundle)
        {
            bundle.Add(new StyleBundle("~/bundle/css").Include(
                                            "~/Content/css/bootstrap.min.css",
                                            "~/Content/css/font-awesome.min.css", 
                                            "~/Content/css/elegant-icons.css",
                                            "~/Content/css/magnific-popup.css",
                                            "~/Content/css/nice-select.css",
                                            "~/Content/css/owl.carousel.min.css",
                                            "~/Content/css/slicknav.min.css",
                                            "~/Content/css/style.css",
                                            "~/Content/css/404.css",
                                            "~/Content/css/formUser.css"
            ));
            bundle.Add(new StyleBundle("~/bundle/admin/login/css").Include(
                                            "~/Areas/Admin/Content/css/style.min.css"
            ));
            bundle.Add(new StyleBundle("~/bundle/adminCss").Include(
                                            "~/Areas/Admin/Content/assets/extra-libs/c3/c3.min.css",
                                            "~/Areas/Admin/Content/assets/libs/chartist/dist/chartist.min.css",
                                            "~/Areas/Admin/Content/assets/extra-libs/jvector/jquery-jvectormap-2.0.2.css",
                                            "~/Areas/Admin/Content/css/style.min.css"
            ));
            bundle.Add(new ScriptBundle("~/bundle/js").Include(
                                            "~/Content/js/jquery-3.3.1.min.js",
                                            "~/Content/js/bootstrap.min.js",
                                            "~/Content/js/jquery.nice-select.min.js",
                                            "~/Content/js/jquery.nicescroll.min.js",
                                            "~/Content/js/jquery.magnific-popup.min.js",
                                            "~/Content/js/jquery.countdown.min.js",
                                            "~/Content/js/jquery.slicknav.js",
                                            "~/Content/js/mixitup.min.js",
                                            "~/Content/js/owl.carousel.min.js",
                                            "~/Content/js/main.js"
             ));
            bundle.Add(new ScriptBundle("~/bundle/adminJs").Include(
                                "~/Areas/Admin/Content/assets/libs/jquery/dist/jquery.min.js",
                                "~/Areas/Admin/Content/assets/libs/popper.js/dist/umd/popper.min.js",
                                "~/Areas/Admin/Content/assets/libs/bootstrap/dist/js/bootstrap.min.js",
                                "~/Areas/Admin/Content/js/app-style-switcher.js",
                                "~/Areas/Admin/Content/js/feather.min.js",
                                "~/Areas/Admin/Content/assets/libs/perfect-scrollbar/dist/perfect-scrollbar.jquery.min.js",
                                "~/Areas/Admin/Content/js/sidebarmenu.js",
                                "~/Areas/Admin/Content/js/custom.min.js",
                                "~/Areas/Admin/Content/assets/extra-libs/c3/d3.min.js",
                                "~/Areas/Admin/Content/assets/extra-libs/c3/c3.min.js",
                                "~/Areas/Admin/Content/assets/extra-libs/jvector/jquery-jvectormap-2.0.2.min.js",
                                "~/Areas/Admin/Content/assets/extra-libs/jvector/jquery-jvectormap-world-mill-en.js",
                                "~/Areas/Admin/Content/js/pages/dashboards/dashboard1.min.js",
                                "~/Areas/Admin/Content/assets/extra-libs/sparkline/sparkline.js"
            ));
            bundle.Add(new ScriptBundle("~/bundle/admin/login/js").Include(
                                "~/Areas/Admin/Content/assets/libs/jquery/dist/jquery.min.js",
                                "~/Areas/Admin/Content/assets/libs/popper.js/dist/umd/popper.min.js",
                                "~/Areas/Admin/Content/assets/libs/bootstrap/dist/js/bootstrap.min.js"
            ));
        }

    }
}