<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="vmware_net.Default" MasterPageFile="vmware-net.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="blueprint_div" class="container" runat="server">
        <asp:Panel ID="Login_Panel" runat="server">
            <div id="login_panel_div">
                <asp:Label ID="lblLoginPanelTitle" runat="server" Text="Enter Credentials"></asp:Label>
                <br />
                <!--
                    Remove the controls between these two commment blocks 
                    to have the server autoloaded from the web.config
                    -->
                <asp:Label ID="lblSdkServer" runat="server" Text="SDK Server"></asp:Label>
                &nbsp;<asp:TextBox ID="txtSdkServer" runat="server"></asp:TextBox>
                <br />
                <!--
                    End Here
                    -->
                <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
                &nbsp;<asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                &nbsp;<asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                <br />
                <asp:Button ID="cmdConnect" runat="server" Text="Connect" OnClick="cmdConnect_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="Vm_Panel" runat="server" Visible="False">
            <div id="vm_panel_div">
                <asp:Label ID="lblVmPanelTitle" runat="server" Text="Provisioning Information"></asp:Label>
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
                &nbsp;<asp:DropDownList ID="cboClusters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboClusters_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblDatastore" runat="server" Text="Datastore"></asp:Label>
                &nbsp;<asp:DropDownList ID="cboDatastores" runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblPortGroups" runat="server" Text="Port Groups"></asp:Label>
                &nbsp;<asp:DropDownList ID="cboPortGroups" runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblResourcePool" runat="server" Text="Resource Pool"></asp:Label>
                &nbsp;<asp:DropDownList ID="cboResourcePools" runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblCpu" runat="server" Text="CPU"></asp:Label>
                &nbsp;<asp:DropDownList ID="cboCpus" runat="server">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblRam" runat="server" Text="Ram"></asp:Label>
                &nbsp;<asp:DropDownList ID="cboRam" runat="server">
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
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
                <asp:Button ID="cmdProvision" runat="server" Text="Provision" OnClick="cmdProvision_Click" />
                <br />
            </div>
        </asp:Panel>
        <asp:Panel ID="Results_Panel" runat="server" Visible="False">
            <div id="results_panel_div">
                <asp:TextBox ID="txtResults" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                <br />
                <asp:Button ID="cmdClose" runat="server" Text="Close" OnClick="cmdClose_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="Error_Panel" runat="server" BorderStyle="Solid" Visible="False" Height="640px" Width="480px">
            <div id="error_panel_div" style="height: 100%">
                <asp:TextBox ID="txtErrors" runat="server" Height="90%" Width="99%" TextMode="MultiLine" ForeColor="Red" Font-Bold="True" BorderStyle="None"></asp:TextBox>
                <br />
                <asp:Button ID="cmdReturn" runat="server" Text="Return" OnClick="cmdReturn_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
