<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="vmware_net.Default" MasterPageFile="vmware-net.Master" %>

<asp:Content ID="Form" ContentPlaceHolderID="vmware_net_content" runat="server">
    <div id="blueprint_div" class="container" runat="server">
        <asp:Panel ID="Login_Panel" runat="server">
            <div id="login_panel_div">
                <div id="lp_Panel_Label" class="span-24 last" style="text-align: center">
                    <asp:Label ID="lblLoginPanelTitle" runat="server" Text="Enter Credentials" Font-Bold="True"></asp:Label>
                    <br />
                </div>
                <div id="lp_spacer-left" class="span-6">&nbsp;</div>
                <div id="lp_control" class="span-12">
                    <!--
                    Remove the controls between these two commment blocks 
                    to have the server autoloaded from the web.config
                    -->
                    <div class="vmn_label">SDK Server</div>
                    <asp:TextBox ID="txtSdkServer" runat="server"></asp:TextBox><br />
                    <!--
                    End Here
                    -->
                    <div class="vmn_label">Username</div>
                    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox><br />
                    <div class="vmn_label">Password</div>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
                    <div class="vmn_label">&nbsp;</div>
                    <asp:Button ID="cmdConnect" runat="server" Text="Connect" OnClick="cmdConnect_Click" /><br />
                </div>
                <div id="lp_spacer-right" class="span-6 last">&nbsp;</div>
            </div>
        </asp:Panel>
        <asp:Panel ID="Vm_Panel" runat="server" Visible="False">
            <div id="vm_panel_div">
                <div id="vm_Panel_Label" class="span-24 last" style="text-align: center">
                    <asp:Label ID="lblVmPanelTitle" runat="server" Text="Provisioning Information" Font-Bold="True"></asp:Label>
                    <br />
                </div>
                <div id="vm_spacer-left" class="span-6">&nbsp;</div>
                <div id="vm_control" class="span-12">
                    <div class="vmn_label">Source VM</div>
                    <asp:DropDownList ID="cboSourceVms" runat="server"></asp:DropDownList><br />
                    <div class="vmn_label">Template</div>
                    <asp:CheckBox ID="chkTemplate" runat="server" Checked="True" />
                    <div class="vmn_label">Target VM</div>
                    <asp:TextBox ID="txtTargetVm" runat="server"></asp:TextBox><br />
                    <div class="vmn_label">OS Customization</div>
                    <asp:DropDownList ID="cboCustomizations" runat="server"></asp:DropDownList><br />
                    <div class="vmn_label">Cluster</div>
                    <asp:DropDownList ID="cboClusters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboClusters_SelectedIndexChanged"></asp:DropDownList><br />
                    <div class="vmn_label">Datastore</div>
                    <asp:DropDownList ID="cboDatastores" runat="server"></asp:DropDownList><br />
                    <div class="vmn_label">Port Group</div>
                    <asp:DropDownList ID="cboPortGroups" runat="server"></asp:DropDownList><br />
                    <div class="vmn_label">Resource Pool</div>
                    <asp:DropDownList ID="cboResourcePools" runat="server"></asp:DropDownList><br />
                    <div class="vmn_label">CPU</div>
                    <asp:DropDownList ID="cboCpus" runat="server">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                    </asp:DropDownList><br />
                    <div class="vmn_label">RAM</div>
                    <asp:DropDownList ID="cboRam" runat="server">
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                    </asp:DropDownList><br />
                    <div class="vmn_label">IP Address</div>
                    <asp:TextBox ID="txtIpAddress" runat="server"></asp:TextBox><br />
                    <div class="vmn_label">Subnet</div>
                    <asp:TextBox ID="txtSubnet" runat="server"></asp:TextBox><br />
                    <div class="vmn_label">Gateway</div>
                    <asp:TextBox ID="txtGateway" runat="server"></asp:TextBox><br />
                    <div class="vmn_label">Dns</div>
                    <asp:TextBox ID="txtDnsServer" runat="server"></asp:TextBox><br />
                    <div class="vmn_label">&nbsp;</div>
                    <asp:Button ID="cmdProvision" runat="server" Text="Provision" OnClick="cmdProvision_Click" />
                </div>
                <div id="vm_spacer-right" class="span-6 last">&nbsp;</div>
            </div>
        </asp:Panel>
        <asp:Panel ID="Results_Panel" runat="server" Visible="False" BackColor="White">
            <div id="results_panel_div">
                <div id="rp_Panel_Label" class="span-24 last" style="text-align: center">
                    <asp:Label ID="lblResultsLabel" runat="server" Text="Results" Font-Bold="True"></asp:Label>
                </div>
                <br />
                <div class="span-6">&nbsp;</div>
                <div id="rp_control" class="span-12">
                    <asp:TextBox ID="txtResults" runat="server" TextMode="MultiLine" Width="100%" ReadOnly="True" BackColor="White"></asp:TextBox><br />
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClick="cmdClose_Click" />
                </div>
                <div class="span-6 last">&nbsp;</div>
            </div>
        </asp:Panel>
        <asp:Panel ID="Error_Panel" runat="server" Visible="False" BackColor="White">
            <div id="error_panel_div">
                <div id="ep_Panel_Label" class="span-24 last" style="text-align: center">
                    <asp:Label ID="lblErrorPanel" runat="server" Text="Error Messages" Font-Bold="True"></asp:Label>
                </div>
                <br />
                <div class="span-6">&nbsp;</div>
                <div id="ep_control" class="span-12">
                    <asp:TextBox ID="txtErrors" runat="server" TextMode="MultiLine" ForeColor="Red" Font-Bold="True" BorderStyle="None" ReadOnly="True" BackColor="White" Width="100%"></asp:TextBox><br />
                    <asp:Button ID="cmdReturn" runat="server" Text="Return" OnClick="cmdReturn_Click" />
                </div>
                <div class="span-6 last">&nbsp;</div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
