<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>


	<xsl:template match="Kingdom[@id='khuzait']"/>
	<xsl:template match="Kingdom[@id='vlandia']"/>
	<xsl:template match="Kingdom[@id='empire']"/>
	<xsl:template match="Kingdom[@id='aserai']"/>
	<xsl:template match="Kingdom[@id='sturgia']"/>
	<xsl:template match="Kingdom[@id='battania']"/>


</xsl:stylesheet>