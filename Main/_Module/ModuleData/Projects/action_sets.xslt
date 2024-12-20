<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output omit-xml-declaration="yes" />
    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()" />
        </xsl:copy>
    </xsl:template>

  
    <xsl:template match='action_set[@id="as_human_warrior"]'>
        <xsl:copy>
            <xsl:copy-of select="@*" />
            <xsl:copy-of select="*" />

			
			
		  <action type="act_chariot_stand_for_movement_data" animation="chariot_rider_stand_1" />
          <action type="act_chariot_stand_1" animation="chariot_rider_stand_1" />
          <action type="act_chariot_idle_1" animation="chariot_rider_stand_1" />
          <action type="act_chariot_forward_walk_stand" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_walk" animation="chariot_rider_forward_walk_stand" />
		  <action type="act_chariot_forward_walk_strafe_right" animation="chariot_rider_forward_walk_stand" />
		  <action type="act_chariot_forward_walk_strafe_left" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_trot_stand" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_trot" animation="chariot_rider_forward_walk_stand" />
		  <action type="act_chariot_forward_trot_strafe_right" animation="chariot_rider_forward_walk_stand" />
		  <action type="act_chariot_forward_trot_strafe_left" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_canter_stand" animation="chariot_rider_forward_canter_stand" />
          <action type="act_chariot_forward_canter" animation="chariot_rider_forward_canter_stand" />
		  <action type="act_chariot_forward_canter_strafe_right" animation="chariot_rider_forward_walk_stand" />
		  <action type="act_chariot_forward_canter_strafe_left" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_gallop_right_foot_stand" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_gallop_right_foot" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_gallop_left_foot_stand" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_forward_gallop_left_foot" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_turn_right" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_turn_left" animation="chariot_rider_forward_walk_stand" />
          <action type="act_chariot_backward_walk_stand" animation="chariot_rider_backward_walk_stand" />
          <action type="act_chariot_backward_walk" animation="chariot_rider_backward_walk_stand" />
		  <action type="act_mount_chariot_from_left" animation="chariot_mount_rider_from_right" />
          <action type="act_mount_chariot_fast_from_left" animation="chariot_mount_rider_from_right" />
          <action type="act_mount_chariot_from_right" animation="chariot_mount_rider_from_right" />
          <action type="act_mount_chariot_fast_from_right" animation="chariot_mount_rider_from_right" />
			

		  <action type="act_ready_overswing_spear_elephant" animation="ready_overswing_spear_elephant" />
		  <action type="act_quick_release_overswing_spear_elephant" animation="quick_release_overswing_spear_elephant" />
		  <action type="act_release_overswing_spear_elephant" animation="release_overswing_spear_elephant" />

			<action type="act_assassin_perform" animation="assassin_perform" />
			<action type="act_assassin_victim" animation="assassin_victim" />

			<action type="act_dual_ready_thrust_1h" animation="ready_dual_thrust_1h" />
			<action type="act_dual_quick_release_thrust_1h" animation="quick_release_dual_thrust_1h" />
			<action type="act_dual_quick_release_thrust_1h_balanced" animation="quick_release_dual_thrust_1h_balanced" />
			<action type="act_dual_release_thrust_1h" animation="release_dual_thrust_1h" />
			<action type="act_dual_release_thrust_1h_balanced" animation="release_dual_thrust_1h_balanced" />
			<action type="act_dual_quick_blocked_thrust_1h" animation="quick_dual_blocked_thrust_1h" />
			<action type="act_dual_quick_blocked_thrust_1h_balanced" animation="quick_dual_blocked_thrust_1h_balanced" />
			<action type="act_dual_blocked_thrust_1h" animation="blocked_thrust_1h" />
			<action type="act_dual_blocked_thrust_1h_balanced" animation="blocked_thrust_1h_balanced" />
			<action type="act_dual_stuck_thrust_1h" animation="stuck_thrust_1h" />
			<action type="act_dual_stuck_quick_thrust_1h" animation="stuck_quick_thrust_1h" />
			<action type="act_dual_ready_thrust_1h_left_stance" animation="ready_thrust_1h_left_stance" />
			<action type="act_dual_quick_release_thrust_1h_left_stance" animation="quick_release_thrust_1h_left_stance" />
			<action type="act_dual_quick_release_thrust_1h_balanced_left_stance" animation="quick_release_thrust_1h_balanced_left_stance" />
			<action type="act_dual_release_thrust_1h_left_stance" animation="release_thrust_1h_left_stance" />
			<action type="act_dual_release_thrust_1h_balanced_left_stance" animation="release_thrust_1h_balanced_left_stance" />
			<action type="act_dual_quick_blocked_thrust_1h_left_stance" animation="quick_blocked_thrust_1h_left_stance" />
			<action type="act_dual_quick_blocked_thrust_1h_balanced_left_stance" animation="quick_blocked_thrust_1h_balanced_left_stance" />
			<action type="act_dual_blocked_thrust_1h_left_stance" animation="blocked_thrust_1h_left_stance" />
			<action type="act_dual_blocked_thrust_1h_balanced_left_stance" animation="blocked_thrust_1h_balanced_left_stance" />
			<action type="act_dual_stuck_thrust_1h_left_stance" animation="stuck_thrust_1h_left_stance" />
			<action type="act_dual_stuck_quick_thrust_1h_left_stance" animation="stuck_quick_thrust_1h_left_stance" />




			<action type="act_dual_ready_slashright_1h" animation="ready_slashright_1h" />
			<action type="act_dual_ready_from_up_slashright_1h" animation="ready_from_up_slashright_1h" />
			<action type="act_dual_ready_from_up_slashright_1h_unbalanced" animation="ready_from_up_slashright_1h_unbalanced" />
			<action type="act_dual_ready_from_right_slashright_1h" animation="ready_from_right_slashright_1h" />
			<action type="act_dual_ready_from_right_slashright_1h_unbalanced" animation="ready_from_right_slashright_1h_unbalanced" />

			<action type="act_dual_quick_release_slashright_1h" animation="quick_release_dual_slashright_1h" />
			<action type="act_dual2_quick_release_slashright_1h" animation="quick_release_dual2_slashright_1h" />

			<action type="act_dual_quick_release_slashright_1h_balanced" animation="quick_release_dual_slashright_1h_balanced" />
			<action type="act_dual2_quick_release_slashright_1h_balanced" animation="quick_release_dual2_slashright_1h_balanced" />

			<action type="act_dual_release_slashright_1h" animation="release_slashright_1h" />
			<action type="act_dual_release_slashright_1h_balanced" animation="release_slashright_1h_balanced" />
			<action type="act_dual_quick_blocked_slashright_1h" animation="quick_blocked_slashright_1h" />
			<action type="act_dual_quick_blocked_slashright_1h_balanced" animation="quick_blocked_slashright_1h_balanced" />

			<action type="act_dual_blocked_slashright_1h" animation="quick_dual_blocked_slashright_1h" />
			<action type="act_dual_blocked_slashright_1h_balanced" animation="quick_dual_blocked_slashright_1h_balanced" />

			<action type="act_dual_ready_slashright_1h_left_stance" animation="ready_slashright_1h_left_stance" />
			<action type="act_dual_ready_from_up_slashright_1h_left_stance" animation="ready_from_up_slashright_1h_left_stance" />
			<action type="act_dual_ready_from_up_slashright_1h_unbalanced_left_stance" animation="ready_from_up_slashright_1h_left_stance_unbalanced" />
			<action type="act_dual_ready_from_right_slashright_1h_left_stance" animation="ready_from_right_slashright_1h_left_stance" />
			<action type="act_dual_ready_from_right_slashright_1h_unbalanced_left_stance" animation="ready_from_right_slashright_1h_left_stance_unbalanced" />
			<action type="act_dual_quick_release_slashright_1h_left_stance" animation="quick_release_slashright_1h_left_stance" />
			<action type="act_dual_quick_release_slashright_1h_balanced_left_stance" animation="quick_release_slashright_1h_balanced_left_stance" />
			<action type="act_dual_release_slashright_1h_left_stance" animation="release_slashright_1h_left_stance" />
			<action type="act_dual_release_slashright_1h_balanced_left_stance" animation="release_slashright_1h_balanced_left_stance" />
			<action type="act_dual_quick_blocked_slashright_1h_left_stance" animation="quick_blocked_slashright_1h_left_stance" />
			<action type="act_dual_quick_blocked_slashright_1h_balanced_left_stance" animation="quick_blocked_slashright_1h_balanced_left_stance" />
			<action type="act_dual_blocked_slashright_1h_left_stance" animation="blocked_slashright_1h_left_stance" />
			<action type="act_dual_blocked_slashright_1h_balanced_left_stance" animation="blocked_slashright_1h_balanced_left_stance" />













			<action type="act_dual_ready_slashleft_1h" animation="ready_dual_slashleft_1h" />
			<action type="act_dual_ready_from_up_slashleft_1h" animation="ready_dual_slashleft_1h" />
			<action type="act_dual_ready_from_up_slashleft_1h_unbalanced" animation="ready_dual_slashleft_1h" />
			<action type="act_dual_ready_from_left_slashleft_1h" animation="ready_from_left_slashleft_1h" />
			<action type="act_dual_ready_from_left_slashleft_1h_unbalanced" animation="ready_from_left_slashleft_1h_unbalanced" />

			<action type="act_dual_quick_release_slashleft_1h" animation="quick_release_dual_slashleft_1h" />
			<action type="act_dual_quick_release_slashleft_1h_balanced" animation="quick_release_dual_slashleft_1h_balanced" />

			<action type="act_dual_release_slashleft_1h" animation="release_slashleft_1h" />
			<action type="act_dual_release_slashleft_1h_balanced" animation="release_slashleft_1h_balanced" />

			<action type="act_dual_quick_blocked_slashleft_1h" animation="quick_dual_blocked_slashleft_1h" />
			<action type="act_dual_quick_blocked_slashleft_1h_balanced" animation="quick_dual_blocked_slashleft_1h_balanced" />

			<action type="act_dual_blocked_slashleft_1h" animation="blocked_slashleft_1h" />
			<action type="act_dual_blocked_slashleft_1h_balanced" animation="blocked_slashleft_1h_balanced" />
			<action type="act_dual_ready_slashleft_1h_left_stance" animation="ready_slashleft_1h_left_stance" />
			<action type="act_dual_ready_from_up_slashleft_1h_left_stance" animation="ready_from_up_slashleft_1h_left_stance" />
			<action type="act_dual_ready_from_up_slashleft_1h_unbalanced_left_stance" animation="ready_from_up_slashleft_1h_left_stance_unbalanced" />
			<action type="act_dual_ready_from_left_slashleft_1h_left_stance" animation="ready_from_left_slashleft_1h_left_stance" />
			<action type="act_dual_ready_from_left_slashleft_1h_unbalanced_left_stance" animation="ready_from_left_slashleft_1h_left_stance_unbalanced" />
			<action type="act_dual_quick_release_slashleft_1h_left_stance" animation="quick_release_slashleft_1h_left_stance" />
			<action type="act_dual_quick_release_slashleft_1h_balanced_left_stance" animation="quick_release_slashleft_1h_balanced_left_stance" />
			<action type="act_dual_release_slashleft_1h_left_stance" animation="release_slashleft_1h_left_stance" />
			<action type="act_dual_release_slashleft_1h_balanced_left_stance" animation="release_slashleft_1h_balanced_left_stance" />
			<action type="act_dual_quick_blocked_slashleft_1h_left_stance" animation="quick_blocked_slashleft_1h_left_stance" />
			<action type="act_dual_quick_blocked_slashleft_1h_balanced_left_stance" animation="quick_blocked_slashleft_1h_balanced_left_stance" />
			<action type="act_dual_blocked_slashleft_1h_left_stance" animation="blocked_slashleft_1h_left_stance" />
			<action type="act_dual_blocked_slashleft_1h_balanced_left_stance" animation="blocked_slashleft_1h_balanced_left_stance" />









			<action type="act_walk_idle_1h_with_d_shld" animation="dual_stand_1h" />
			<action type="act_walk_forward_1h_with_d_shld" animation="dual_walk_forward_1h" />
			<action type="act_walk_idle_1h_with_d_shld_left_stance" animation="dual_stand_1h_left_stance" />
			<action type="act_walk_forward_1h_with_d_shld_left_stance" animation="dual_walk_forward_1h_left_stance" />
			<action type="act_run_idle_1h_with_d_shld" animation="dual_stand_1h" />
			<action type="act_run_forward_1h_with_d_shld" animation="dual_run_forward_1h_with_hand_shield" />
			<action type="act_run_idle_1h_with_d_shld_left_stance" animation="dual_stand_1h_left_stance" />
			<action type="act_run_forward_1h_with_d_shld_left_stance" animation="dual_run_forward_1h_with_hand_shield_left_stance" />





			<action type="act_walk_idle_1h_drunk" animation="anim_drinker_conversation" />
			<action type="act_walk_forward_1h_drunk" animation="drinker_walk_forward_drunk" />
			<action type="act_run_idle_1h_drunk" animation="anim_drinker_conversation" />
			<action type="act_run_forward_1h_drunk" animation="drinker_walk_forward_drunk" />

			<action type="act_drink_mead_drunk" animation="anim_drink_drunk" />
			<action type="act_drink_mead_drunk_1h" animation="anim_drink_drunk" />
			<action type="act_drink_mead_drunk_1h_balanced" animation="anim_drink_drunk" />







			<action type="act_coward_prerun" animation="anim_coward_prerun" />
			
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>