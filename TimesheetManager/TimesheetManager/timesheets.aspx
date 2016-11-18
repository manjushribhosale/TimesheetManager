<%@ Page Title="" Language="C#" MasterPageFile="~/TimesheetManager.Master" AutoEventWireup="true" CodeBehind="timesheets.aspx.cs" Inherits="TimesheetManager.timesheets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#timesheetDate").change(function () {
                document.getElementById('<%= closeDayHidden.ClientID %>').value = document.getElementById('timesheetDate').value;
                document.forms[0].submit();
            });
        });

        // **************************************************
        // FORM ADD VALIDATION AND SUBMIT
        function validateForm(inValue) {
            var regExInt = /^[0-9]{1,2}$/ //regular expression
            var errorMsg = "Please complete the following fields:<br><br>";

            var hoursTotal = 0;
            var minutesTotal = 0;
            errorCnt = 0;
            
            for (var j = 0; j < document.getElementById("<%=rowCounterHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("desc_" + j + "")[0].value == "") {
                    errorMsg += "* Description row " + (j + 1) + "<br>";
                    errorCnt++;
                }
            }

            for (var j = 0; j < document.getElementById("<%=rowCounterHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("category_" + j + "")[0].value == "") {
                    errorMsg += "* Category row " + (j + 1) + "<br>";
                    errorCnt++;
                }
            }

            for (var j = 0; j < document.getElementById("<%=rowCounterHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("hours_" + j + "")[0].value != "") {
                    if (document.getElementsByName("hours_" + j + "")[0].value.search(regExInt) == -1) {
                        errorMsg += "* Hours Row " + (j + 1) + " is not fomatted correctly<br>";
                        errorCnt++;
                    }
                    else {
                        if (document.getElementById("tmDeleteHidden_" + j + "").value == "0") {
                            hoursTotal = Number(hoursTotal) + Number(document.getElementsByName("hours_" + j + "")[0].value);
                        }
                    }
                }
                else {
                    if (document.getElementsByName("minutes_" + j + "")[0].value == "") {
                        errorMsg += "* You must enter Hours or Minutes for Row " + (j + 1) + "<br>";
                        errorCnt++;
                    }
                }
            }

            for (var j = 0; j < document.getElementById("<%=rowCounterHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("minutes_" + j + "")[0].value != "") {
                    if (document.getElementsByName("minutes_" + j + "")[0].value.search(regExInt) == -1) {
                        errorMsg += "* Minutes Row " + (j + 1) + " is not fomatted correctly<br>";
                        errorCnt++;
                    }
                    else {
                        if (document.getElementById("tmDeleteHidden_" + j + "").value == "0") {
                            minutesTotal = Number(minutesTotal) + Number(document.getElementsByName("minutes_" + j + "")[0].value);
                        }
                    }
                }
            }

            for (var j = 1; j <= document.getElementById("<%=newRowsHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("newDesc_" + j + "")[0].value.trim() != "" || document.getElementsByName("newCategory_" + j + "")[0].value != "" || document.getElementsByName("newClient_" + j + "")[0].value != "" || document.getElementsByName("newHours_" + j + "")[0].value != "" || document.getElementsByName("newMinutes_" + j + "")[0].value != "") {
                    if (document.getElementsByName("newDesc_" + j + "")[0].value == "") {
                        errorMsg += "* Description New Entry row " + (j) + "<br>";
                        errorCnt++;
                    }
                }
            }

            for (var j = 1; j <= document.getElementById("<%=newRowsHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("newDesc_" + j + "")[0].value.trim() != "" || document.getElementsByName("newCategory_" + j + "")[0].value != "" || document.getElementsByName("newClient_" + j + "")[0].value != "" || document.getElementsByName("newHours_" + j + "")[0].value != "" || document.getElementsByName("newMinutes_" + j + "")[0].value != "") {
                    if (document.getElementsByName("newCategory_" + j + "")[0].value == "") {
                        errorMsg += "* Category New Entry row " + (j) + "<br>";
                        errorCnt++;
                    }
                }
            }

            for (var j = 1; j <= document.getElementById("<%=newRowsHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("newHours_" + j + "")[0].value != "") {
                    if (document.getElementsByName("newHours_" + j + "")[0].value.search(regExInt) == -1) {
                        errorMsg += "* Hours New Entry Row " + (j) + " is not fomatted correctly<br>";
                        errorCnt++;
                    }
                    else {
                        hoursTotal = Number(hoursTotal) + Number(document.getElementsByName("newHours_" + j + "")[0].value);
                    }
                }
                else {
                    if (document.getElementsByName("newDesc_" + j + "")[0].value != "" || document.getElementsByName("newCategory_" + j + "")[0].value != "" || document.getElementsByName("newClient_" + j + "")[0].value != "" || document.getElementsByName("newHours_" + j + "")[0].value != "" || document.getElementsByName("newMinutes_" + j + "")[0].value != "") {
                        if (document.getElementsByName("newMinutes_" + j + "")[0].value == "") {
                            errorMsg += "* You must enter Hours or Minutes for New Entry Row " + (j) + "<br>";
                            errorCnt++;
                        }
                    }
                }
            }

            for (var j = 1; j <= document.getElementById("<%=newRowsHidden.ClientID%>").value; j++) {
                if (document.getElementsByName("newMinutes_" + j + "")[0].value != "") {
                    if (document.getElementsByName("newMinutes_" + j + "")[0].value.search(regExInt) == -1) {
                        errorMsg += "* Minutes New Entry Row " + (j) + " is not fomatted correctly<br>";
                        errorCnt++;
                    }
                    else {
                        minutesTotal = Number(minutesTotal) + Number(document.getElementsByName("newMinutes_" + j + "")[0].value);
                    }
                }
            }

            hoursTotal = Number(hoursTotal) + Number(document.getElementById("<%=taskTotalHoursHidden.ClientID%>").value);
            minutesTotal = Number(minutesTotal) + Number(document.getElementById("<%=taskTotalMinutesHidden.ClientID%>").value);

            var additionalHours = 0;
            var newMinutes = 0;

            //ADDING IN ANY HOURS AND MINUTES FROM TASK TIME
            if (minutesTotal >= 60)
            {
                additionalHours = minutesTotal / 60;
                newMinutes = minutesTotal - (Math.floor(additionalHours) * 60);

                minutesTotal = newMinutes;
                hoursTotal = hoursTotal + additionalHours;
            }

            if (hoursTotal >= 24) {
                errorMsg += "* You cannot exceed 24 hours for total time worked in a single day.<br>";
                errorCnt++;
            }

            if (inValue == "close") {
                if (Number(hoursTotal) < Number(document.getElementById("<%=minHoursHidden.ClientID%>").value)) {
                    errorMsg += "* You must enter at least " + document.getElementById("<%=minHoursHidden.ClientID%>").value + " hours in order to close the selected date.<br>";
                    errorCnt++;
                }
            }

            // IF ERROR EXISTS THEN DISPLAY ALERT MESSAGE
            if (errorCnt > 0) {
                jAlert(errorMsg, "Alert");
                return false;
            }
            else{
                //SETTING VALUE TO SIGNIFY IF THE UPDATE OR THE UPDATE AND CLOSE BUTTON WAS PRESSED.
                document.getElementById("<%=submitTypeHidden.ClientID%>").value = inValue;
            }

        }

        //AFTER USER CHANGES A HOUR OR MINUTE VALUE RE EVALUATE THE TOTAL TIME AND DISPLAY NEW TIME
        $(document).ready(function () {
            $(document).on("focusout", ".form_text_field_timesheet_sm", function () {
                var myId = $(this).attr('id');
                var aa = myId.split('_');

                var hoursUpdated = 0;
                var minutesUpdated = 0;
                var additionalUpdatedHours = 0;
                var newUpdatedMinutes = 0;
                //TIMESHEET HOUR ROWS
                for (var j = 0; j < document.getElementById("<%=rowCounterHidden.ClientID%>").value; j++) {
                    if (document.getElementById("tmDeleteHidden_" + j + "").value == "0") {
                        hoursUpdated = Number(hoursUpdated) + Number(document.getElementsByName("hours_" + j + "")[0].value);
                    }
                }

                //TIMESHEET MINUTE ROWS
                for (var j = 0; j < document.getElementById("<%=rowCounterHidden.ClientID%>").value; j++) {
                    if (document.getElementById("tmDeleteHidden_" + j + "").value == "0") {
                        if (document.getElementsByName("minutes_" + j + "")[0].value >= 60) {
                            document.getElementsByName("minutes_" + j + "")[0].value = "59";
                        }

                        minutesUpdated = Number(minutesUpdated) + Number(document.getElementsByName("minutes_" + j + "")[0].value);
                    }
                }

                //NEW TIMESHEET HOUR ROWS
                for (var j = 1; j <= document.getElementById("<%=newRowsHidden.ClientID%>").value; j++) {

                    hoursUpdated = Number(hoursUpdated) + Number(document.getElementsByName("newHours_" + j + "")[0].value);
                }

                //NEW TIMESHEET MINUTE ROWS
                for (var j = 1; j <= document.getElementById("<%=newRowsHidden.ClientID%>").value; j++) {

                    if (document.getElementsByName("newMinutes_" + j + "")[0].value >= 60) {
                        document.getElementsByName("newMinutes_" + j + "")[0].value = "59";
                    }

                    minutesUpdated = Number(minutesUpdated) + Number(document.getElementsByName("newMinutes_" + j + "")[0].value);
                }

                hoursUpdated = Number(hoursUpdated) + Number(document.getElementById("<%=taskTotalHoursHidden.ClientID%>").value);
                minutesUpdated = Number(minutesUpdated) + Number(document.getElementById("<%=taskTotalMinutesHidden.ClientID%>").value);

                if (minutesUpdated >= 60) {
                    additionalUpdatedHours = minutesUpdated / 60;
                    newUpdatedMinutes = minutesUpdated - (Math.floor(additionalUpdatedHours) * 60);
                    minutesUpdated = newUpdatedMinutes;
                    hoursUpdated = hoursUpdated + Math.floor(additionalUpdatedHours);
                }

                var updatedRemainingHours = 0;
                var updatedRemainingMinutes = 0;
                var minHours = document.getElementById("<%=minHoursHidden.ClientID%>").value;

                if (minHours > hoursUpdated) {

                    if (minutesUpdated > 0) {
                        updatedRemainingHours = minHours - (hoursUpdated + 1);
                    }
                    else
                    {
                        updatedRemainingHours = minHours - hoursUpdated;
                    }
                    
                    if (minutesUpdated > 0) {
                        updatedRemainingMinutes = 60 - minutesUpdated;
                    } else {
                        updatedRemainingMinutes = "00";
                    }
                }

                if (updatedRemainingHours == 0 && updatedRemainingMinutes == 00) {
                    $('#ContentPlaceHolder1_totalHoursDiv').css({ "color": "#37aa00" });
                } else {
                    $('#ContentPlaceHolder1_totalHoursDiv').css({ "color": "#bd0a0a" });
                }

                document.getElementById("totalTime").innerHTML = Math.floor(hoursUpdated) + ":" + zeroPad(minutesUpdated, 2);
                document.getElementById("remainingHours").innerHTML = updatedRemainingHours + ":" + zeroPad(updatedRemainingMinutes, 2);
            });
        });
        // **************************************************


        // **************************************************
        function zeroPad(num, places) {
            var zero = places - num.toString().length + 1;
            return Array(+(zero > 0 && zero)).join("0") + num;
        }
        // **************************************************


        // **************************************************
        var counter = 0;
        function displayNewRow() {
            var clientList = "";
            var projectList = "";
            var categoryList = "";

            $.ajax({
                type: "post",
                url: "timesheets.aspx/GetClientAJAX",
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d) {
                        clientList = result.d;
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });

            $.ajax({
                type: "post",
                url: "timesheets.aspx/GetProjectAJAX",
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d) {
                        projectList = result.d;
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });

            setTimeout(function(){ 
                $.ajax({
                    type: "post",
                    url: "timesheets.aspx/GetCategoryAJAX",
                    data: JSON.stringify({}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.d) {
                            categoryList = result.d;

                            counter++;
                            if (document.getElementById("<%=displayClientDropdownsHidden.ClientID%>").value != "No") {
                            document.getElementById("timesheetTable").insertRow(-1).innerHTML = '<tr> \
                                <tr><td> \
                                <textarea class="form_textarea" id="newDesc_' + counter + '" name="newDesc_' + counter + '" cols="50" rows="2"></textarea>\
                                </td><td>\
                                <select id="newCategory_' + counter + '" name="newCategory_' + counter + '" class="form_select_list_sm"><option value="">Choose Category</option>' + categoryList + '</select>\
                                </td><td text-align="center">\
                                <div><select id="newClient_' + counter + '" name="newClient_' + counter + '" class="form_select_list_sm"><option value="">Choose Client</option>' + clientList + '</select></div><br />\
                                <div><select id="newProject_' + counter + '" name="newProject_' + counter + '" class="form_select_list_sm"><option value="">Choose Project</option>' + projectList + '</select></div>\
                                </td><td text-align="center">\
                                <input type="text" size="2" style="text-align:center;" maxlength="2" id="newHours_' + counter + '" name="newHours_' + counter + '" value="" class="form_text_field_timesheet_sm"></input>\
                                </td><td>\
                                <input type="text" size="2" style="text-align:center;" maxlength="2" id="newMinutes_' + counter + '" name="newMinutes_' + counter + '" value="" class="form_text_field_timesheet_sm"></input>\
                                </td><td></td></tr>';
                            }else{
                                document.getElementById("timesheetTable").insertRow(-1).innerHTML = '<tr> \
                                <tr><td> \
                                <textarea class="form_textarea" id="newDesc_' + counter + '" name="newDesc_' + counter + '" cols="60" rows="2"></textarea>\
                                </td><td>\
                                <select id="newCategory_' + counter + '" name="newCategory_' + counter + '" class="form_select_list_sm"><option value="">Choose Category</option>' + categoryList + '</select>\
                                </td><td text-align="center">\
                                <div><select id="newProject_' + counter + '" name="newProject_' + counter + '" class="form_select_list_sm"><option value="">Choose Project</option>' + projectList + '</select></div>\
                                </td><td text-align="center">\
                                <input type="text" size="2" style="text-align:center;" maxlength="2" id="newHours_' + counter + '" name="newHours_' + counter + '" value="" class="form_text_field_timesheet_sm"></input>\
                                </td><td>\
                                <input type="text" size="2" style="text-align:center;" maxlength="2" id="newMinutes_' + counter + '" name="newMinutes_' + counter + '" value="" class="form_text_field_timesheet_sm"></input>\
                                </td><td></td></tr>';

                            }
                            document.getElementById("<%=newRowsHidden.ClientID%>").value = counter;  
                        }
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });
            }, 1000)
        }
        // **************************************************

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="body_wrapper">
		<div id="mid_body_container">
			<div id="container_header">TIMESHEET - CLOSE DAY &nbsp;&nbsp;&nbsp;&nbsp;<asp:ValidationSummary id="valSum" DisplayMode="SingleParagraph" EnableClientScript="true" HeaderText="Update failed. Please verify your information." runat="server"/><asp:Label ID="msgLabel" runat="server" Text=""></asp:Label></div>
			<div id="body_copy">
                <asp:HiddenField ID="rowCounterHidden" runat="server" />
                <asp:HiddenField ID="taskTotalHoursHidden" runat="server" />
                <asp:HiddenField ID="taskTotalMinutesHidden" runat="server" />
                <asp:HiddenField ID="minHoursHidden" runat="server" />
                <asp:HiddenField ID="closeDayHidden" runat="server" />
                <asp:HiddenField ID="submitTypeHidden" runat="server" />
                <asp:HiddenField ID="newRowsHidden" Value="0" runat="server" />
                <asp:HiddenField ID="displayClientDropdownsHidden" Value="No" runat="server" />


				<div id="row1Col">
					<div>Fill out the following timesheets for the day. To delete timesheet records click the trash can for each record and then click "UPDATE" or "UPDATE AND CLOSE" to confirm all changes.</div>
                    <br />
                    <div id="timesheetDateWrapper">
                        DATE:&nbsp;&nbsp;&nbsp;&nbsp;
                        <select class="form_select_list_sm" id="timesheetDate" name="timesheetDate">
                            <option value="">Choose One</option>
                            <asp:Label runat="server" ID="closeDaysLabel" value=""></asp:Label>
                        </select>
                    </div>
                    <div id="selectedDate"><asp:Label ID="selectedDateLabel" runat="server" Text=""></asp:Label></div>
				</div>
					
                <div id="row1Col">
                    <table class='timesheet_table' id="timesheetTable" cellspacing='0' cellpadding='0' style="border-collapse:collapse">
                        <tr>
                            <th style='text-align:left; width:425px;'>DESCRIPTION</th>
                            <th style='text-align:left; width:130px;'>CATEGORY</th>
                            <th style='text-align:left; width:300px;'>CLIENT / PROJECT</th>
                            <th style='text-align:center; width:45px;'>HRS</th>
                            <th style='text-align:center; width:40px;'>MIN</th>
                            <th style='text-align:center; width:20px;'>ACTION</th>
                        </tr>
                        <asp:Label ID="timeListLabel" runat="server" Text=""></asp:Label>
                    </table>
                    <asp:Panel runat="server" ID="totalHoursPanel" Visible="false">
                    <table class='timesheet_table' id="Table1" cellspacing='0' cellpadding='0' style="border-top: none; border-collapse:collapse">
                    <tr>
                        <td colspan="1" style="text-align:left; vertical-align:top; width:450px;">
                            <input type="button" id="addTimesheetRecordButton" class="button" name="addTimesheetRecordButton" onclick="displayNewRow();" value="ADD MORE TIME" />
                        </td>
                        <td colspan="4" style="text-align:right; padding-top:20px; padding-bottom:15px; padding-right:15px; width:515px;">
                            <div id="totalHoursDiv" runat="server">TOTAL HOURS: <div id="totalTime"><asp:Label ID="totalHrsLabel" runat="server" Text="0"></asp:Label>:<asp:Label ID="totalMinLabel" runat="server" Text="00"></asp:Label></div></div>
                            <div id="remainingHoursDiv">REMAINING: <div id="remainingHours"><asp:Label ID="remainingHrsLabel" runat="server" Text="0"></asp:Label>:<asp:Label ID="remainingMinLabel" runat="server" Text="00"></asp:Label></div></div>
                            <div id="minimumHoursDiv">Minimum Hours Required: <asp:Label ID="minHoursLabel" runat="server" Text="0"></asp:Label></div>
                        </td>
                    </tr>
                    </table>
                    </asp:Panel> 
                </div>
                <asp:Panel runat="server" ID="submitPagePanel" Visible="false">
                <div id="rowButtons" style="border:none; text-align:right; margin-bottom:40px; margin-top:0px;">
                    <asp:LinkButton type="reset" class="button" OnClientClick="document.forms[0].reset();return false;" ID="resetButton" runat="server">RESET</asp:LinkButton>
                    <asp:LinkButton type="button" class="button" OnClientClick="return validateForm('update');" OnClick="buttonSubmit_Click" ID="buttonSubmit" runat="server">UPDATE</asp:LinkButton>
                    <asp:LinkButton type="button" class="button" OnClientClick="return validateForm('close');" OnClick="buttonSubmit_Click" ID="buttonSubmitClose" runat="server">UPDATE AND CLOSE</asp:LinkButton>
                </div>
                </asp:Panel>
				<br/><br/><br/><br/>
			</div>
		</div>
	</div>
    <script type="text/javascript">
        //if (document.getElementById('<%= closeDayHidden.ClientID %>').value != null && document.getElementById('<%= closeDayHidden.ClientID %>').value != "") {
        //    displayNewRow();
        //}
    </script>
</asp:Content>
