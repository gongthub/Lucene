<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" method="get" action="Default.aspx">
        <div>
            请输入搜索关键字:<input type="text" name="SearchKey" value="" />
            <input type="submit" name="btnCreate" value="创建索引" />
            <input type="submit" name="btnSearch" value="一哈哈" />
            <br />
            <ul>
                <asp:ListView ID="ListView1" runat="server">

                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Id") %></td>
                            <td><%# Eval("title") %></td>
                            <td><%# Eval("ContentDescription") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </ul>
        </div>
    </form>
</body>
</html>
