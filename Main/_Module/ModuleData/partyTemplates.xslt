<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output omit-xml-declaration="yes"/>
<xsl:template match="@*|node()">
	<xsl:copy>
		<xsl:apply-templates select="@*|node()"/>
	</xsl:copy>
</xsl:template>

	<xsl:template match="MBPartyTemplate[@id='kingdom_hero_party_vlandia_template']">
		<MBPartyTemplate id="kingdom_hero_party_vlandia_template" >
			<stacks>
				<PartyTemplateStack min_value="16" max_value="16" troop="NPCCharacter.vlandian_recruit" />
				<PartyTemplateStack min_value="8" max_value="8" troop="NPCCharacter.vlandian_footman" />
				<PartyTemplateStack min_value="4" max_value="5" troop="NPCCharacter.vlandian_infantry" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.vlandian_swordsman" />
				<PartyTemplateStack min_value="1" max_value="1" troop="NPCCharacter.vlandian_sergeant" />
				<PartyTemplateStack min_value="8" max_value="8" troop="NPCCharacter.vlandian_levy_crossbowman" />
				<PartyTemplateStack min_value="4" max_value="4" troop="NPCCharacter.vlandian_crossbowman" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.vlandian_hardened_crossbowman" />
				<PartyTemplateStack min_value="1" max_value="1" troop="NPCCharacter.vlandian_sharpshooter" />
				<PartyTemplateStack min_value="1" max_value="1" troop="NPCCharacter.vlandian_banner_knight" />
				<PartyTemplateStack min_value="4" max_value="4" troop="NPCCharacter.vlandian_light_cavalry" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.vlandian_vanguard" />
			</stacks>
		</MBPartyTemplate>
	</xsl:template>

	<xsl:template match="MBPartyTemplate[@id='kingdom_hero_party_empire_template']">
		<MBPartyTemplate id="kingdom_hero_party_empire_template" >
			<stacks>
				<PartyTemplateStack min_value="16" max_value="16" troop="NPCCharacter.imperial_recruit" />
				<PartyTemplateStack min_value="8" max_value="8" troop="NPCCharacter.imperial_infantryman" />
				<PartyTemplateStack min_value="4" max_value="4" troop="NPCCharacter.imperial_trained_infantryman" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.imperial_veteran_infantryman" />
				<PartyTemplateStack min_value="1" max_value="1" troop="NPCCharacter.imperial_legionary" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.imperial_menavliaton" />
				<PartyTemplateStack min_value="4" max_value="4" troop="NPCCharacter.imperial_equite" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.imperial_heavy_horseman" />
				<PartyTemplateStack min_value="1" max_value="1" troop="NPCCharacter.imperial_elite_cataphract" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.bucellarii" />

				<PartyTemplateStack min_value="8" max_value="8" troop="NPCCharacter.imperial_archer" />
				<PartyTemplateStack min_value="4" max_value="4" troop="NPCCharacter.imperial_trained_archer" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.imperial_veteran_archer" />
				<PartyTemplateStack min_value="1" max_value="1" troop="NPCCharacter.imperial_palatine_guard" />
				<PartyTemplateStack min_value="4" max_value="4" troop="NPCCharacter.imperial_crossbowman" />
				<PartyTemplateStack min_value="2" max_value="2" troop="NPCCharacter.imperial_cataphract" />
			</stacks>
		</MBPartyTemplate>
	</xsl:template>


</xsl:stylesheet>

