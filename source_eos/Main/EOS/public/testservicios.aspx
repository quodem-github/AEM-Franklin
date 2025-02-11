<%@ Page Language="C#" AutoEventWireup="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <script type="text/javascript" src="/Scripts/jQuery/jquery-1.7.2.min.js" ></script>
        <script type="text/javascript" src="/Scripts/jQuery/jquery.simplemodal.js"></script>
        <script type="text/javascript" src="/Scripts/jquery-ui-1.11.4.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function() {
                callServices();
                callServices2();
                callServices3();
            });

            function callServices() {
                $.ajax({
                    type: "GET",
                    url: "http://eosmsd.com.quodem.net/gp/api/public/IsTokenManagement.aspx?param=EosPubKey",
                    cache: false,
                    success: function (response) {
                        var data = {
                            "Data":
                                "RoO3WMl/7qHUh6T7ev8ebH0x0x+02AS60/Wulka52HB5UyniwPeRrApiOeHMmceY1wVAe+R7I1a3buHkLKNvPluxyoFTvUPaffpS5kXZhCAiVJ9AD5kXwbamdRzJXVOcKhtE5os3JkHfRXO40c30BGE5PLmHbJbB7018GO8KCjUA2LbbHMwspB01XDSBFoM1gI7yW2U90MPTPUpQqn0ZM9r8jGxn2QBQLU9zDwHurlfAwZhIz0DXCUlz8fXzcfh4lZ047VfQD69QssOesV6eYtcFQHvkeyNW6PbloaLTCZSwOkANo7VHlDpzMcjzJdwWYFPiQtb8W+k="
                        };
                        $.ajax({
                            type: "POST",
                            url: "http://eosmsd.com.quodem.net/gp/api/Estados_Reservas/List",
                            headers: {
                                "Content-Type": 'application/json',
                                "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                            },
                            data: JSON.stringify(data),
                            contentType: 'application/json',
                            success: function (response) {
                                var dataproveedores = { "Data": "pM1jqYgeju101MgxiDsNoqPR8uWgKthXMVAt4ARGRKmoAjrhVLV6uLdu4eQso28+l9W3NHLCP2fOhU4+WOkCWjn8Ft8aCZzKaE7gIFqE1n8FH8vhnDBDZxCMiPHFnUK68p9Byvk2GWDo1J4MfAbKVBVB5xu+5KLCMrf0p0E6SkGjioOsQ2YUNi+HWTKU5NPWMVAt4ARGRKk8jAclP6Yr/Ng4mL3TwU+sKcKphRunDO//mKyrH8wjs6/IAkd10vb13WaG7zUNnElA4A2AbqWbSFVS6QWQqC0nlHHBbmq9Xa6mFdibBto0r7du4eQso28+55qYGmo/gG3X/1ORra9dUPJxEQpO3cj+U8IHyrvJDL+fGO4MvV6FkH7bcNPmK28It7iPtONWd6MgiDiDAozbRLFRXXRzey6tPpWzxxIg7t6itTt/7ZqjB3f5O3LOuAH8K54jHzqhUKOwe0PyxMvoIJ9KtH3ByEssjtI+5aKS/b2B/jl+m78wszMKjG8nM9UoTOcaGOgoCrg=" }

                                $.ajax({
                                    type: "POST",
                                    url: "http://eosmsd.com.quodem.net/gp/api/Proveedor/Edit",
                                    headers: {
                                        "Content-Type": 'application/json',
                                        "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                        "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                                    },
                                    data: JSON.stringify(dataproveedores),
                                    contentType: 'application/json',
                                    success: function (response) {

                                    },
                                    error: function (request, status, error) {
                                        console.log('Error edición de proveedores: ' + request.responseText);
                                    }
                                });
                            },
                            error: function (request, status, error) {
                                console.log('Error listado de reservas: ' + request.responseText);
                            }
                        });
                    },
                    error: function (request, status, error) {
                        console.log('Error obtenión de token: ' + request.responseText);
                    }
                });
                setTimeout(callServices, 600);
            }

            function callServices2() {
                $.ajax({
                    type: "GET",
                    url: "http://eosmsd.com.quodem.net/gp/api/public/IsTokenManagement.aspx?param=EosPubKey",
                    cache: false,
                    success: function (response) {
                        var data = {
                            "Data":
                                "RoO3WMl/7qHUh6T7ev8ebH0x0x+02AS60/Wulka52HB5UyniwPeRrApiOeHMmceY1wVAe+R7I1a3buHkLKNvPluxyoFTvUPaffpS5kXZhCAiVJ9AD5kXwbamdRzJXVOcKhtE5os3JkHfRXO40c30BGE5PLmHbJbB7018GO8KCjUA2LbbHMwspB01XDSBFoM1gI7yW2U90MPTPUpQqn0ZM9r8jGxn2QBQLU9zDwHurlfAwZhIz0DXCUlz8fXzcfh4lZ047VfQD69QssOesV6eYtcFQHvkeyNW6PbloaLTCZSwOkANo7VHlDpzMcjzJdwWYFPiQtb8W+k="
                        };
                        $.ajax({
                            type: "POST",
                            url: "http://eosmsd.com.quodem.net/gp/api/Estados_Reservas/List",
                            headers: {
                                "Content-Type": 'application/json',
                                "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                            },
                            data: JSON.stringify(data),
                            contentType: 'application/json',
                            success: function (response) {
                                var dataproveedores = { "Data": "pM1jqYgeju101MgxiDsNoqPR8uWgKthXMVAt4ARGRKmoAjrhVLV6uLdu4eQso28+l9W3NHLCP2fOhU4+WOkCWjn8Ft8aCZzKaE7gIFqE1n8FH8vhnDBDZxCMiPHFnUK68p9Byvk2GWDo1J4MfAbKVBVB5xu+5KLCMrf0p0E6SkGjioOsQ2YUNi+HWTKU5NPWMVAt4ARGRKk8jAclP6Yr/Ng4mL3TwU+sKcKphRunDO//mKyrH8wjs6/IAkd10vb13WaG7zUNnElA4A2AbqWbSFVS6QWQqC0nlHHBbmq9Xa6mFdibBto0r7du4eQso28+55qYGmo/gG3X/1ORra9dUPJxEQpO3cj+U8IHyrvJDL+fGO4MvV6FkH7bcNPmK28It7iPtONWd6MgiDiDAozbRLFRXXRzey6tPpWzxxIg7t6itTt/7ZqjB3f5O3LOuAH8K54jHzqhUKOwe0PyxMvoIJ9KtH3ByEssjtI+5aKS/b2B/jl+m78wszMKjG8nM9UoTOcaGOgoCrg=" }

                                $.ajax({
                                    type: "POST",
                                    url: "http://eosmsd.com.quodem.net/gp/api/Proveedor/Edit",
                                    headers: {
                                        "Content-Type": 'application/json',
                                        "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                        "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                                    },
                                    data: JSON.stringify(dataproveedores),
                                    contentType: 'application/json',
                                    success: function (response) {

                                    },
                                    error: function (request, status, error) {
                                        console.log('Error edición de proveedores: ' + request.responseText);
                                    }
                                });
                            },
                            error: function (request, status, error) {
                                console.log('Error listado de reservas: ' + request.responseText);
                            }
                        });
                    },
                    error: function (request, status, error) {
                        console.log('Error obtenión de token: ' + request.responseText);
                    }
                });

                $.ajax({
                    type: "GET",
                    url: "http://eosmsd.com.quodem.net/gp/api/public/IsTokenManagement.aspx?param=EosPubKey",
                    cache: false,
                    success: function (response) {
                        var data = {
                            "Data":
                                "RoO3WMl/7qHUh6T7ev8ebH0x0x+02AS60/Wulka52HB5UyniwPeRrApiOeHMmceY1wVAe+R7I1a3buHkLKNvPluxyoFTvUPaffpS5kXZhCAiVJ9AD5kXwbamdRzJXVOcKhtE5os3JkHfRXO40c30BGE5PLmHbJbB7018GO8KCjUA2LbbHMwspB01XDSBFoM1gI7yW2U90MPTPUpQqn0ZM9r8jGxn2QBQLU9zDwHurlfAwZhIz0DXCUlz8fXzcfh4lZ047VfQD69QssOesV6eYtcFQHvkeyNW6PbloaLTCZSwOkANo7VHlDpzMcjzJdwWYFPiQtb8W+k="
                        };
                        $.ajax({
                            type: "POST",
                            url: "http://eosmsd.com.quodem.net/gp/api/Estados_Reservas/List",
                            headers: {
                                "Content-Type": 'application/json',
                                "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                            },
                            data: JSON.stringify(data),
                            contentType: 'application/json',
                            success: function (response) {
                                var dataproveedores = { "Data": "pM1jqYgeju101MgxiDsNoqPR8uWgKthXMVAt4ARGRKmoAjrhVLV6uLdu4eQso28+l9W3NHLCP2fOhU4+WOkCWjn8Ft8aCZzKaE7gIFqE1n8FH8vhnDBDZxCMiPHFnUK68p9Byvk2GWDo1J4MfAbKVBVB5xu+5KLCMrf0p0E6SkGjioOsQ2YUNi+HWTKU5NPWMVAt4ARGRKk8jAclP6Yr/Ng4mL3TwU+sKcKphRunDO//mKyrH8wjs6/IAkd10vb13WaG7zUNnElA4A2AbqWbSFVS6QWQqC0nlHHBbmq9Xa6mFdibBto0r7du4eQso28+55qYGmo/gG3X/1ORra9dUPJxEQpO3cj+U8IHyrvJDL+fGO4MvV6FkH7bcNPmK28It7iPtONWd6MgiDiDAozbRLFRXXRzey6tPpWzxxIg7t6itTt/7ZqjB3f5O3LOuAH8K54jHzqhUKOwe0PyxMvoIJ9KtH3ByEssjtI+5aKS/b2B/jl+m78wszMKjG8nM9UoTOcaGOgoCrg=" }

                                $.ajax({
                                    type: "POST",
                                    url: "http://eosmsd.com.quodem.net/gp/api/Proveedor/Edit",
                                    headers: {
                                        "Content-Type": 'application/json',
                                        "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                        "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                                    },
                                    data: JSON.stringify(dataproveedores),
                                    contentType: 'application/json',
                                    success: function (response) {

                                    },
                                    error: function (request, status, error) {
                                        console.log('Error edición de proveedores: ' + request.responseText);
                                    }
                                });
                            },
                            error: function (request, status, error) {
                                console.log('Error listado de reservas: ' + request.responseText);
                            }
                        });
                    },
                    error: function (request, status, error) {
                        console.log('Error obtenión de token: ' + request.responseText);
                    }
                });

                $.ajax({
                    type: "GET",
                    url: "http://eosmsd.com.quodem.net/gp/api/public/IsTokenManagement.aspx?param=EosPubKey",
                    cache: false,
                    success: function (response) {
                        var data = {
                            "Data":
                                "RoO3WMl/7qHUh6T7ev8ebH0x0x+02AS60/Wulka52HB5UyniwPeRrApiOeHMmceY1wVAe+R7I1a3buHkLKNvPluxyoFTvUPaffpS5kXZhCAiVJ9AD5kXwbamdRzJXVOcKhtE5os3JkHfRXO40c30BGE5PLmHbJbB7018GO8KCjUA2LbbHMwspB01XDSBFoM1gI7yW2U90MPTPUpQqn0ZM9r8jGxn2QBQLU9zDwHurlfAwZhIz0DXCUlz8fXzcfh4lZ047VfQD69QssOesV6eYtcFQHvkeyNW6PbloaLTCZSwOkANo7VHlDpzMcjzJdwWYFPiQtb8W+k="
                        };
                        $.ajax({
                            type: "POST",
                            url: "http://eosmsd.com.quodem.net/gp/api/Estados_Reservas/List",
                            headers: {
                                "Content-Type": 'application/json',
                                "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                            },
                            data: JSON.stringify(data),
                            contentType: 'application/json',
                            success: function (response) {
                                var dataproveedores = { "Data": "pM1jqYgeju101MgxiDsNoqPR8uWgKthXMVAt4ARGRKmoAjrhVLV6uLdu4eQso28+l9W3NHLCP2fOhU4+WOkCWjn8Ft8aCZzKaE7gIFqE1n8FH8vhnDBDZxCMiPHFnUK68p9Byvk2GWDo1J4MfAbKVBVB5xu+5KLCMrf0p0E6SkGjioOsQ2YUNi+HWTKU5NPWMVAt4ARGRKk8jAclP6Yr/Ng4mL3TwU+sKcKphRunDO//mKyrH8wjs6/IAkd10vb13WaG7zUNnElA4A2AbqWbSFVS6QWQqC0nlHHBbmq9Xa6mFdibBto0r7du4eQso28+55qYGmo/gG3X/1ORra9dUPJxEQpO3cj+U8IHyrvJDL+fGO4MvV6FkH7bcNPmK28It7iPtONWd6MgiDiDAozbRLFRXXRzey6tPpWzxxIg7t6itTt/7ZqjB3f5O3LOuAH8K54jHzqhUKOwe0PyxMvoIJ9KtH3ByEssjtI+5aKS/b2B/jl+m78wszMKjG8nM9UoTOcaGOgoCrg=" }

                                $.ajax({
                                    type: "POST",
                                    url: "http://eosmsd.com.quodem.net/gp/api/Proveedor/Edit",
                                    headers: {
                                        "Content-Type": 'application/json',
                                        "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                                        "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                                    },
                                    data: JSON.stringify(dataproveedores),
                                    contentType: 'application/json',
                                    success: function (response) {

                                    },
                                    error: function (request, status, error) {
                                        console.log('Error edición de proveedores: ' + request.responseText);
                                    }
                                });
                            },
                            error: function (request, status, error) {
                                console.log('Error listado de reservas: ' + request.responseText);
                            }
                        });
                    },
                    error: function (request, status, error) {
                        console.log('Error obtenión de token: ' + request.responseText);
                    }
                });
                setTimeout(callServices2, 10000);
            }


            function callServices3() {

                var dataPassengers = { "Data": "yfPR9zHBeIRW+2pbBhMJQe2wFUhC7XnFAZvwm/Xpg6HJ0MIokYytXVLSyShzLSLrlmOG2UDyjr+pzopzqRyTnaq0sfsFoiaT1NpjCl7ZH0+P8CCq/cM+tZo51axuAvdM" }

                $.ajax({
                    type: "POST",
                    url: "http://eosmsd.com.quodem.net/gp/api/PassengersList.aspx",
                    headers: {
                        "Content-Type": 'application/json',
                        "X-Token": '0C66EDE172D0DD3805B6328D46DE629F77A794E7',
                        "Agency-Key": 'P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg=='
                    },
                    data: JSON.stringify(dataPassengers),
                    contentType: 'application/json',
                    success: function (response) {

                    },
                    error: function (request, status, error) {
                        console.log('Error edición de proveedores: ' + request.responseText);
                    }
                });
                            
                setTimeout(callServices3, 15000);
            }
        </script>
    </div>
    </form>
</body>
</html>
