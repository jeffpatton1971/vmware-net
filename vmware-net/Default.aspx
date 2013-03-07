<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="vmware_net.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Virtual Machine Provisioning Site</title>
</head>
<body>
    <form id="frmProvision" runat="server">
        <div id="login_panel">
            <asp:Label ID="lblLoginPanelTitle" runat="server" Text="Enter Credentials"></asp:Label>
            <br />
            <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
            &nbsp;<asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
            &nbsp;
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            <asp:Button ID="cmdConnect" runat="server" Text="Connect" />
        </div>
        <div id="vm_panel">
            <asp:Label ID="lblVmPanelTitle" runat="server" Text="Provisioning Information"></asp:Label>
            <br />
            <asp:Label ID="lblSdkServer" runat="server" Text="SDK Server"></asp:Label>
            &nbsp;<asp:TextBox ID="txtSdkServer" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblSourceVm" runat="server" Text="Source VM"></asp:Label>
            &nbsp;<asp:DropDownList ID="cboSourceVms" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblTargetVm" runat="server" Text="Target VM"></asp:Label>
            &nbsp;<asp:TextBox ID="txtTargetVm" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblOsCustomization" runat="server" Text=" OS Customization"></asp:Label>
            &nbsp;<asp:DropDownList ID="cboCustomizations" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblCluster" runat="server" Text="Cluster"></asp:Label>
            &nbsp;<asp:DropDownList ID="cboClusters" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblDatastore" runat="server" Text="Datastore"></asp:Label>
            &nbsp;<asp:DropDownList ID="cboDatastores" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblNetwork" runat="server" Text="Network"></asp:Label>
            &nbsp;<asp:DropDownList ID="cboPortGroups" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblCpu" runat="server" Text="CPU"></asp:Label>
            &nbsp;<asp:DropDownList ID="cboCpus" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblRam" runat="server" Text="Ram"></asp:Label>
            &nbsp;<asp:DropDownList ID="cboRam" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblIpAddress" runat="server" Text="Ip Address"></asp:Label>
            &nbsp;<asp:TextBox ID="txtIpAddress" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblSubnet" runat="server" Text="Subnet"></asp:Label>
            &nbsp;<asp:TextBox ID="txtSubnet" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblGateway" runat="server" Text="Gateway"></asp:Label>
            &nbsp;<asp:TextBox ID="txtGateway" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="cmdProvision" runat="server" Text="Provision" />
            <br />
        </div>
    </form>
</body>
</html>
