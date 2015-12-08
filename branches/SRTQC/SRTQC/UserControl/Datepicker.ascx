<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Datepicker.ascx.cs" Inherits="UserControl_Datepicker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="TextBox1" Format="yyyy/MM/dd" />
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

