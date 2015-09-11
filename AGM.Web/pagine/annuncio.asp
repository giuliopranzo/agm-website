<!--#include virtual="/include/header.asp"-->
<%
set annunci = db.execute("select * from annunci where idannuncio = "&request.querystring("idannuncio")&" ")
if annunci.eof then
	Response.Status="301 Moved Permanently"
	Response.AddHeader "Location","http://"&Request.ServerVariables("HTTP_HOST")&"/ricerca-personale-posizioni-aperte/"
else
	file = Server.MapPath("/annunci/"&annunci("idannuncio")&".txt")
	set objFile = fso.OpenTextFile(file, 1)

%>
<head>
<title><%=annunci("titolo")%> | AGM Solutions</title>
<meta name="description" content="<%=annunci("dove")%>. <%=replace(left(objFile.ReadLine,130),"<p>","")%> ...">
</head>
<%
	objFile.close
response.write "<h3>Area di ricerca: "&annunci("dove")&"</h3>"
response.write "<h1>"&annunci("titolo")&"</h1>"
	set objFile = fso.OpenTextFile(file, 1)
	Do While Not objFile.AtEndofStream
		Response.Write objFile.ReadLine
	Loop
	objFile.close
%>
<p>Le ricerche sono rivolte a candidati dell'uno e dell'altro sesso ai sensi della L. 903/77 e L. 125/91.</p>
<p>Inviare cv completo all'indirizzo di posta elettronica
<a href="mailto:hr@agmsolutions.net?subject=rif. <%=annunci("riferimento")%>"><b>hr@agmsolutions.net</b></a>, citando nell’oggetto il seguente riferimento: <b><%=annunci("riferimento")%>.</b></p>
<% end if %>
<!--#include virtual="/Include/footer.asp"-->