<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="PDFtoHTMLAndView.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="//jqueryui.com/jquery-wp-content/themes/jqueryui.com/style.css">
    <script type="text/javascript">

        $(document).ready(function () {

            $('#btnUploadFile').on('click', function () {

                // jquery Progress bar function. 
                $("#progressbar").progressbar({ value: 0 });
                var progressbar = $("#progressbar");

                progressbar.progressbar({
                    value: false,
                    complete: function () {
                        progressLabel.text("Complete!");
                    }
                });
                document.getElementById('product').innerHTML = '';
                var data = new FormData();

                var files = $("#fileUpload").get(0).files;

                // Add the uploaded image content to the form data collection
                if (files.length > 0) {
                    data.append("UploadedPDF", files[0]);
                }
                // Make Ajax request with the contentType = false, and procesDate = false
                var ajaxRequest = $.ajax({
                    type: "POST",
                    url: "http://localhost:5879/api/PDFtoHTML/ConvertPDFtoHTMLAndView",
                    contentType: false,
                    processData: false,
                    data: data
                });

                ajaxRequest.done(function (responseData, textStatus) {
                    if (textStatus == 'success') {
                        if (responseData != null) {
                            if (responseData.Key) {
                                $("#fileUpload").val('');
                                document.getElementById('product').innerHTML = responseData.Value;
                                $("#progressbar").progressbar({ value: 100 });
                            } else {
                                $('#product').text(responseData.Value);
                            }
                        }
                    } else {
                        $('#product').text(responseData.Value);
                    }
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td align="center" valign="top">
                    <table style="background: #000; min-width: 900px;">
                        <tr style="color: #fff; margin: 5px;">
                            <td colspan="3" align="center">
                                <h1>PDF to HTML Converter
                                    <br />
                                    Web API using Aspose.Pdf & JQuery</h1>
                            </td>
                        </tr>
                        <tr style="color: #fff">
                            <td><b>Select PDF File</b><br />
                                <input type="file" id="fileUpload" />
                            </td>
                            <td>
                                <input type="button" value="Upload & View in HTML" id="btnUploadFile" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="progressbar"></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" valign="top" style="background: #494646;">

                                <b>&nbsp; HTML VIEW</b><br />
                                <div id="product" style="border: solid 1px #808080; margin: 15px; background: #fff;"></div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>

    </form>
</body>
</html>
