<%@ Page Title="" Language="C#" MasterPageFile="~/login.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="TimesheetManager.login1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="login_box">
		<div id="login_logo"></div>
		<div id="login_error">
            <asp:ValidationSummary id="valSum" DisplayMode="SingleParagraph" EnableClientScript="true" HeaderText="Login Failed. Please try again!" runat="server"/>
            <asp:Label ID="msgLabel" runat="server" Text=""></asp:Label></div>
        <asp:TextBox ID="usernameLogin" CssClass="field_username" Placeholder="Username or Email Address" runat="server" MaxLength="100"></asp:TextBox>
        <asp:Regularexpressionvalidator id="nameRegex" runat="server" 
            ControlToValidate="usernameLogin" 
            ValidationExpression="^[a-zA-Z0-9!_@\.\-\/]{1,100}$" 
            ErrorMessage="">
        </asp:Regularexpressionvalidator>
    
        <asp:TextBox TextMode="Password" ID="passwordLogin" CssClass="field_password" Placeholder="**********************" runat="server" MaxLength="50"></asp:TextBox>
        <asp:Regularexpressionvalidator id="Regularexpressionvalidator1" runat="server" 
            ControlToValidate="passwordLogin" 
            ValidationExpression="^.{1,50}$" 
            ErrorMessage="">
        </asp:Regularexpressionvalidator>
        <input class="checkbox_rememberme" type="checkbox" id="checkbox-1-1" name="rememberMe"/><label for="checkbox-1-1"></label>
		<div id="keep_me">KEEP ME LOGGED IN</div>
		<div class="button_login">
            <asp:Button ID="login_submit" OnClick="loginSubmit_Click" runat="server" Text="" />
        </div>
				
		<div id="login_divider"></div>
				
		<div id="login_bottom_wrap">
			<asp:Panel ID="forgotPassPanel" Visible="false" runat="server"><a href="forgot_password.aspx"><div id="login_link">FORGOT PASSWORD</div></a><div id="login_link_divider"></div></asp:Panel>
			<asp:Panel ID="registerPanel" Visible="false" runat="server"><a href="register.aspx"><div id="login_link">CREATE NEW LOGIN</div></a></asp:Panel>
		</div>
	</div>
</asp:Content>
