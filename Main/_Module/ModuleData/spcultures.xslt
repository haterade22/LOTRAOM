<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output omit-xml-declaration="yes"/>
    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()"/>
        </xsl:copy>
    </xsl:template>
	
		<xsl:template match="Culture[@id='vlandia']"/>
		<xsl:template match="Culture[@id='empire']"/>
		<xsl:template match="Culture[@id='aserai']"/>
		<xsl:template match="Culture[@id='sturgia']"/>
		<xsl:template match="Culture[@id='battania']"/>
		<xsl:template match="Culture[@id='khuzait']"/>
		<xsl:template match="Culture[@id='neutral_culture']"/>
		<xsl:template match="Culture[@id='steppe_bandits']"/>
		<xsl:template match="Culture[@id='desert_bandits']"/>
		<xsl:template match="Culture[@id='forest_bandits']"/>
		<xsl:template match="Culture[@id='mountain_bandits']"/>
		<xsl:template match="Culture[@id='sea_raiders']"/>
		<xsl:template match="Culture[@id='looters']"/>
		<xsl:template match="Culture[@id='darshi']"/>
		<xsl:template match="Culture[@id='vakken']"/>
		<xsl:template match="Culture[@id='nord']"/>

</xsl:stylesheet>