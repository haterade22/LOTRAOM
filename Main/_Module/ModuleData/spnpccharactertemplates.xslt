<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="NPCCharacter[@id='villager_empire']"/>
	<xsl:template match="NPCCharacter[@id='villager_khuzait']"/>
	<xsl:template match="NPCCharacter[@id='villager_vlandia']"/>
	<xsl:template match="NPCCharacter[@id='villager_sturgia']"/>
	<xsl:template match="NPCCharacter[@id='villager_aserai']"/>
	<xsl:template match="NPCCharacter[@id='villager_battania']"/>

	<xsl:template match="NPCCharacter[@id='townswoman_empire']"/>
	<xsl:template match="NPCCharacter[@id='townswoman_khuzait']"/>
	<xsl:template match="NPCCharacter[@id='townswoman_vlandia']"/>
	<xsl:template match="NPCCharacter[@id='townswoman_sturgia']"/>
	<xsl:template match="NPCCharacter[@id='townswoman_aserai']"/>
	<xsl:template match="NPCCharacter[@id='townswoman_battania']"/>

	<xsl:template match="NPCCharacter[@id='fighter_empire']"/>
	<xsl:template match="NPCCharacter[@id='fighter_khuzait']"/>
	<xsl:template match="NPCCharacter[@id='fighter_vlandia']"/>
	<xsl:template match="NPCCharacter[@id='fighter_sturgia']"/>
	<xsl:template match="NPCCharacter[@id='fighter_aserai']"/>
	<xsl:template match="NPCCharacter[@id='fighter_battania']"/>

</xsl:stylesheet>
