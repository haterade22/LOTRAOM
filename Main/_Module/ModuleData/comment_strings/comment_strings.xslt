<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output omit-xml-declaration="yes"/>
    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()"/>
        </xsl:copy>
    </xsl:template>

	<xsl:template match="string[@id='str_comment_vassal_introduces_self.vlandia']"/>
	<xsl:template match="string[@id='str_comment_noble_introduces_self.vlandia']"/>
	<xsl:template match="string[@id='str_comment_noble_introduces_self_and_clan.vlandia']"/>
	<xsl:template match="string[@id='str_comment_noble_introduces_self_and_clan.vlandia_honor']"/>
	<xsl:template match="string[@id='str_comment_noble_introduces_self_and_clan.vlandia_mercy']"/>
	<xsl:template match="string[@id='str_comment_noble_introduces_self_and_clan.vlandia_boasting']"/>
	<xsl:template match="string[@id='str_comment_noble_introduces_self_and_clan.vlandia_boasting_cruel']"/>
	<xsl:template match="string[@id='str_comment_noble_introduces_self_and_clan.vlandia_boasting_ironic']"/>
	<xsl:template match="string[@id='str_liege_title.vlandia']"/>
	<xsl:template match="string[@id='str_liege_title_female.vlandia']"/>	
	
	
	</xsl:stylesheet>