@{
    # Each module produces one .sdPlugin / one csproj / one exe.
    # Action 'Type' values:
    #   Simple     - fire-on-press, no telemetry, single state, no icon flip
    #   GearTel    - telemetry-driven 0/100% via "gear, %",   2 states (next-press preview)
    #   FlapsCyc   - telemetry-driven cycle via "flaps, %",   2 states (sweep cycle, direction tracked)
    #   AirbrkTel  - telemetry-driven 0/100% via "airbrake, %", 2 states
    Modules = @(

        @{ Id='mechanisation'; Asm='Mechanisation'; Cat='WT - Mechanisation'; Accent=@(255,180,80);
           Actions=@(
              @{ N='Toggle Gear';        U='gear';         T='GearTel';   B=@('ID_GEAR');                                    L='GEAR' }
              @{ N='Toggle Flaps';       U='flaps';        T='FlapsCyc';  B=@('ID_FLAPS','ID_FLAPS_DOWN','ID_FLAPS_UP');     L='FLAPS' }
              @{ N='Air Brake';          U='airbrake';     T='AirbrkTel'; B=@('ID_AIR_BRAKE');                                L='AIRBRK' }
              @{ N='Air Reverse';        U='air_reverse';  T='Simple';    B=@('ID_AIR_REVERSE');                              L='REV' }
              @{ N='Bomb Bay Door';      U='bay_door';     T='Simple';    B=@('ID_BAY_DOOR');                                 L='BAY' }
              @{ N='Cockpit Door';       U='cockpit_door'; T='Simple';    B=@('ID_TOGGLE_COCKPIT_DOOR');                      L='CKPT' }
              @{ N='Carrier Tail Hook';  U='trans_gear';   T='Simple';    B=@('ID_TRANS_GEAR_DOWN');                          L='HOOK' }
              @{ N='Boosters / WEP';     U='boosters';     T='Simple';    B=@('ID_IGNITE_BOOSTERS');                          L='WEP' }
              @{ N='VTOL Up';            U='vtol_up';      T='Simple';    B=@('vtol_rangeMax');                               L='VTOL+' }
              @{ N='VTOL Down';          U='vtol_down';    T='Simple';    B=@('vtol_rangeMin');                               L='VTOL-' }
           )
        }

        @{ Id='engine'; Asm='Engine'; Cat='WT - Engine'; Accent=@(255,128,48);
           Actions=@(
              @{ N='Throttle Max';       U='throttle_max'; T='Simple';    B=@('throttle_rangeMax');                           L='THR+' }
              @{ N='Throttle Min';       U='throttle_min'; T='Simple';    B=@('throttle_rangeMin');                           L='IDLE' }
              @{ N='Maneuverability Mode'; U='manuv_mode'; T='Simple';    B=@('ID_MANEUVERABILITY_MODE');                     L='MANUV' }
           )
        }

        @{ Id='weapons.aa'; Asm='WeaponsAa'; Cat='WT - Weapons A-A'; Accent=@(224,80,80);
           Actions=@(
              @{ N='Air-to-Air Missile'; U='aam';          T='Simple';    B=@('ID_AAM');                                      L='AAM' }
              @{ N='Fire Primary';       U='fire_primary'; T='Simple';    B=@('ID_FIRE_PRIMARY');                             L='FIRE1' }
              @{ N='Fire Secondary';     U='fire_secondary'; T='Simple';  B=@('ID_FIRE_SECONDARY');                           L='FIRE2' }
              @{ N='Fire Cannons';       U='fire_cannons'; T='Simple';    B=@('ID_FIRE_CANNONS');                             L='CANN' }
              @{ N='Fire MGuns';         U='fire_mguns';   T='Simple';    B=@('ID_FIRE_MGUNS');                               L='MGUN' }
              @{ N='Fire Add. Guns';     U='fire_add_guns'; T='Simple';   B=@('ID_FIRE_ADDITIONAL_GUNS');                     L='ADDG' }
              @{ N='Lock Target';        U='lock_target';  T='Simple';    B=@('ID_LOCK_TARGET');                              L='LOCK' }
              @{ N='Weapon Lock';        U='weapon_lock';  T='Simple';    B=@('ID_WEAPON_LOCK');                              L='WLCK' }
              @{ N='Cycle Primary';      U='cycle_primary'; T='Simple';   B=@('ID_SWITCH_SHOOTING_CYCLE_PRIMARY');            L='CYC1' }
              @{ N='Cycle Secondary';    U='cycle_secondary'; T='Simple'; B=@('ID_SWITCH_SHOOTING_CYCLE_SECONDARY');          L='CYC2' }
              @{ N='Reload Guns';        U='reload';       T='Simple';    B=@('ID_RELOAD_GUNS');                              L='RELD' }
           )
        }

        @{ Id='weapons.ag'; Asm='WeaponsAg'; Cat='WT - Weapons A-G'; Accent=@(192,64,160);
           Actions=@(
              @{ N='Air-to-Ground Missile'; U='agm';       T='Simple';    B=@('ID_AGM');                                      L='AGM' }
              @{ N='AGM Lock';           U='agm_lock';     T='Simple';    B=@('ID_AGM_LOCK');                                 L='AGML' }
              @{ N='ATGM';               U='atgm';         T='Simple';    B=@('ID_ATGM');                                     L='ATGM' }
              @{ N='Drop Bomb';          U='bombs';        T='Simple';    B=@('ID_BOMBS');                                    L='BOMB' }
              @{ N='Bomb Series';        U='bombs_series'; T='Simple';    B=@('ID_BOMBS_SERIES');                             L='BMB^' }
              @{ N='Fire Rockets';       U='rockets';      T='Simple';    B=@('ID_ROCKETS');                                  L='ROCK' }
              @{ N='Guided Bomb Lock';   U='gbu_lock';     T='Simple';    B=@('ID_GUIDED_BOMBS_LOCK');                        L='GBUL' }
              @{ N='Laser Designator';   U='laser';        T='Simple';    B=@('ID_TOGGLE_LASER_DESIGNATOR');                  L='LASER' }
              @{ N='Rocket Ballistics';  U='rocket_bc';    T='Simple';    B=@('ID_TOGGLE_ROCKETS_BALLISTIC_COMPUTER');        L='RBC' }
              @{ N='Combined BC';        U='bc';           T='Simple';    B=@('ID_TOGGLE_CANNONS_AND_ROCKETS_BALLISTIC_COMPUTER'); L='BC' }
              @{ N='Designate Target';   U='designate';    T='Simple';    B=@('ID_DESIGNATE_TARGET');                         L='DSGN' }
              @{ N='Lock Point';         U='lock_pt';      T='Simple';    B=@('ID_LOCK_TARGETING_AT_POINT');                  L='LKPT' }
              @{ N='Unlock Point';       U='unlock_pt';    T='Simple';    B=@('ID_UNLOCK_TARGETING_AT_POINT');                L='ULPT' }
           )
        }

        @{ Id='countermeasures'; Asm='Countermeasures'; Cat='WT - Countermeasures'; Accent=@(240,208,64);
           Actions=@(
              @{ N='Flares';             U='flares';       T='Simple';    B=@('ID_FLARES');                                   L='FLR' }
              @{ N='IR Projector';       U='ir_proj';      T='Simple';    B=@('ID_IR_PROJECTOR');                             L='IRP' }
              @{ N='Smoke Screen';       U='smoke';        T='Simple';    B=@('ID_SMOKE_SCREEN');                             L='SMK' }
           )
        }

        @{ Id='radar.air'; Asm='RadarAir'; Cat='WT - Radar Air'; Accent=@(80,208,112);
           Actions=@(
              @{ N='Radar Power';        U='sensor_switch'; T='Simple';   B=@('ID_SENSOR_SWITCH');                            L='RDR' }
              @{ N='Mode Switch';        U='mode_switch';  T='Simple';    B=@('ID_SENSOR_MODE_SWITCH');                       L='MODE' }
              @{ N='Range Switch';       U='range_switch'; T='Simple';    B=@('ID_SENSOR_RANGE_SWITCH');                      L='RNG' }
              @{ N='Scan Pattern';       U='scan_switch';  T='Simple';    B=@('ID_SENSOR_SCAN_PATTERN_SWITCH');               L='SCAN' }
              @{ N='ACM Mode';           U='acm';          T='Simple';    B=@('ID_SENSOR_ACM_SWITCH');                        L='ACM' }
              @{ N='Target Lock';        U='target_lock';  T='Simple';    B=@('ID_SENSOR_TARGET_LOCK','ID_LOCK_TARGET','ID_LOCK_TARGETING'); L='LOCK' }
              @{ N='Target Switch';      U='target_switch'; T='Simple';   B=@('ID_SENSOR_TARGET_SWITCH');                     L='TGT>' }
              @{ N='Type Switch';        U='type_switch';  T='Simple';    B=@('ID_SENSOR_TYPE_SWITCH');                       L='TYPE' }
              @{ N='Lock Designation';   U='lock_des';     T='Simple';    B=@('ID_LOCK_TARGETING');                           L='LCKD' }
              @{ N='Unlock Designation'; U='unlock_des';   T='Simple';    B=@('ID_UNLOCK_TARGETING');                         L='ULDD' }
           )
        }

        @{ Id='optics.air'; Asm='OpticsAir'; Cat='WT - Optics Air'; Accent=@(160,112,208);
           Actions=@(
              @{ N='Thermal Polarity';   U='thermal';      T='Simple';    B=@('ID_THERMAL_WHITE_IS_HOT');                     L='THM' }
              @{ N='Night Vision';       U='night_vision'; T='Simple';    B=@('ID_PLANE_NIGHT_VISION');                       L='NV' }
              @{ N='Designate Target';   U='designate';    T='Simple';    B=@('ID_DESIGNATE_TARGET');                         L='DSGN' }
              @{ N='Lock at Point';      U='lock_pt';      T='Simple';    B=@('ID_LOCK_TARGETING_AT_POINT');                  L='LKPT' }
              @{ N='Cue X +';            U='cue_x_max';    T='Simple';    B=@('sensor_cue_x_rangeMax');                       L='CUEX+' }
              @{ N='Cue X -';            U='cue_x_min';    T='Simple';    B=@('sensor_cue_x_rangeMin');                       L='CUEX-' }
              @{ N='Cue X Center';       U='cue_x_set';    T='Simple';    B=@('sensor_cue_x_rangeSet');                       L='CUEX0' }
              @{ N='Cue Y +';            U='cue_y_max';    T='Simple';    B=@('sensor_cue_y_rangeMax');                       L='CUEY+' }
              @{ N='Cue Y -';            U='cue_y_min';    T='Simple';    B=@('sensor_cue_y_rangeMin');                       L='CUEY-' }
              @{ N='Cue Y Center';       U='cue_y_set';    T='Simple';    B=@('sensor_cue_y_rangeSet');                       L='CUEY0' }
              @{ N='Cue Z +';            U='cue_z_max';    T='Simple';    B=@('sensor_cue_z_rangeMax');                       L='CUEZ+' }
              @{ N='Cue Z -';            U='cue_z_min';    T='Simple';    B=@('sensor_cue_z_rangeMin');                       L='CUEZ-' }
              @{ N='Cue Z Center';       U='cue_z_set';    T='Simple';    B=@('sensor_cue_z_rangeSet');                       L='CUEZ0' }
              @{ N='Laser Designator';   U='laser';        T='Simple';    B=@('ID_TOGGLE_LASER_DESIGNATOR');                  L='LASER' }
              @{ N='Target Camera';      U='tgt_cam';      T='Simple';    B=@('ID_TARGET_CAMERA');                            L='TGTC' }
              @{ N='UAV Camera';         U='uav_cam';      T='Simple';    B=@('ID_TOGGLE_UAV_CAMERA');                        L='UAV' }
           )
        }

        @{ Id='heli.mech'; Asm='HeliMech'; Cat='WT - Heli Mech'; Accent=@(64,192,160);
           Actions=@(
              @{ N='Heli Gear';          U='gear';         T='Simple';    B=@('ID_GEAR_HELICOPTER');                          L='GEAR' }
              @{ N='Heli Flaps Up';      U='flaps_up';     T='Simple';    B=@('ID_FLAPS_UP_HELICOPTER');                      L='FLP+' }
              @{ N='Heli Flaps Down';    U='flaps_down';   T='Simple';    B=@('ID_FLAPS_DOWN_HELICOPTER');                    L='FLP-' }
              @{ N='Heli Air Brake';     U='airbrake';     T='Simple';    B=@('ID_AIR_BRAKE_HELICOPTER');                     L='AIRB' }
              @{ N='Collective Max';     U='coll_max';     T='Simple';    B=@('helicopter_collective_rangeMax');              L='COL+' }
              @{ N='Collective Min';     U='coll_min';     T='Simple';    B=@('helicopter_collective_rangeMin');              L='COL-' }
              @{ N='Pedals Right';       U='pedal_max';    T='Simple';    B=@('helicopter_pedals_rangeMax');                  L='PED>' }
              @{ N='Pedals Left';        U='pedal_min';    T='Simple';    B=@('helicopter_pedals_rangeMin');                  L='<PED' }
              @{ N='Cyclic Pitch +';     U='cpitch_max';   T='Simple';    B=@('helicopter_cyclic_pitch_rangeMax');            L='PIT+' }
              @{ N='Cyclic Pitch -';     U='cpitch_min';   T='Simple';    B=@('helicopter_cyclic_pitch_rangeMin');            L='PIT-' }
              @{ N='Cyclic Roll +';      U='croll_max';    T='Simple';    B=@('helicopter_cyclic_roll_rangeMax');             L='ROL+' }
              @{ N='Cyclic Roll -';      U='croll_min';    T='Simple';    B=@('helicopter_cyclic_roll_rangeMin');             L='ROL-' }
           )
        }

        @{ Id='heli.combat'; Asm='HeliCombat'; Cat='WT - Heli Combat'; Accent=@(208,80,80);
           Actions=@(
              @{ N='Heli Fire Primary';  U='fire_primary'; T='Simple';    B=@('ID_FIRE_PRIMARY_HELICOPTER');                  L='FIRE1' }
              @{ N='Heli Fire Secondary'; U='fire_secondary'; T='Simple'; B=@('ID_FIRE_SECONDARY_HELICOPTER');                L='FIRE2' }
              @{ N='Heli Fire MGuns';    U='fire_mguns';   T='Simple';    B=@('ID_FIRE_MGUNS_HELICOPTER');                    L='MGUN' }
              @{ N='Heli Fire Cannons';  U='fire_cannons'; T='Simple';    B=@('ID_FIRE_CANNONS_HELICOPTER');                  L='CANN' }
              @{ N='Heli Fire Add Guns'; U='fire_add_guns'; T='Simple';   B=@('ID_FIRE_ADDITIONAL_GUNS_HELICOPTER');          L='ADDG' }
              @{ N='Heli Flares';        U='flares';       T='Simple';    B=@('ID_FLARES_SERIES_HELICOPTER');                 L='FLR' }
              @{ N='Heli Rocket BC';     U='rocket_bc';    T='Simple';    B=@('ID_TOGGLE_ROCKETS_BALLISTIC_COMPUTER_HELICOPTER'); L='RBC' }
              @{ N='Heli Combined BC';   U='bc';           T='Simple';    B=@('ID_TOGGLE_CANNONS_AND_ROCKETS_BALLISTIC_COMPUTER_HELICOPTER'); L='BC' }
              @{ N='Heli Cycle Primary'; U='cycle_primary'; T='Simple';   B=@('ID_SWITCH_SHOOTING_CYCLE_PRIMARY_HELICOPTER'); L='CYC1' }
              @{ N='Heli Cycle Sec';     U='cycle_secondary'; T='Simple'; B=@('ID_SWITCH_SHOOTING_CYCLE_SECONDARY_HELICOPTER'); L='CYC2' }
              @{ N='Heli Instructor';    U='instructor';   T='Simple';    B=@('ID_TOGGLE_INSTRUCTOR_HELICOPTER');             L='INST' }
              @{ N='Heli Exit Cycle';    U='exit_cycle';   T='Simple';    B=@('ID_EXIT_SHOOTING_CYCLE_MODE_HELICOPTER');      L='EXIT' }
           )
        }

        @{ Id='heli.sensors'; Asm='HeliSensors'; Cat='WT - Heli Sensors'; Accent=@(96,160,224);
           Actions=@(
              @{ N='Heli Night Vision';  U='night_vision'; T='Simple';    B=@('ID_HELI_GUNNER_NIGHT_VISION');                 L='NV' }
              @{ N='Seeker Camera';      U='seeker_cam';   T='Simple';    B=@('ID_CAMERA_SEEKER_HELICOPTER');                 L='SKR' }
              @{ N='Sensor Switch';      U='sensor_switch'; T='Simple';   B=@('ID_SENSOR_SWITCH_HELICOPTER');                 L='SNS' }
              @{ N='Sensor Lock';        U='sensor_lock';  T='Simple';    B=@('ID_SENSOR_TARGET_LOCK_HELICOPTER');            L='LOCK' }
              @{ N='Heli Laser';         U='laser';        T='Simple';    B=@('ID_TOGGLE_LASER_DESIGNATOR_HELICOPTER');       L='LASER' }
              @{ N='Heli Target Cam';    U='tgt_cam';      T='Simple';    B=@('ID_TARGET_CAMERA_HELICOPTER');                 L='TGTC' }
              @{ N='Heli Lock Point';    U='lock_pt';      T='Simple';    B=@('ID_LOCK_TARGETING_AT_POINT_HELICOPTER');       L='LKPT' }
              @{ N='Heli Cue X +';       U='cue_x_max';    T='Simple';    B=@('helicopter_sensor_cue_x_rangeMax');            L='CUX+' }
              @{ N='Heli Cue X -';       U='cue_x_min';    T='Simple';    B=@('helicopter_sensor_cue_x_rangeMin');            L='CUX-' }
              @{ N='Heli Cue Y +';       U='cue_y_max';    T='Simple';    B=@('helicopter_sensor_cue_y_rangeMax');            L='CUY+' }
              @{ N='Heli Cue Y -';       U='cue_y_min';    T='Simple';    B=@('helicopter_sensor_cue_y_rangeMin');            L='CUY-' }
           )
        }

        @{ Id='tank.movement'; Asm='TankMovement'; Cat='WT - Tank Move'; Accent=@(160,112,80);
           Actions=@(
              @{ N='Direction Driving';  U='dir_drive';    T='Simple';    B=@('ID_ENABLE_GM_DIRECTION_DRIVING');              L='DRV' }
              @{ N='Suspension Up';      U='susp_clr_up';  T='Simple';    B=@('ID_SUSPENSION_CLEARANCE_UP');                  L='SUSP+' }
              @{ N='Suspension Down';    U='susp_clr_dn';  T='Simple';    B=@('ID_SUSPENSION_CLEARANCE_DOWN');                L='SUSP-' }
              @{ N='Pitch Up';           U='susp_pit_up';  T='Simple';    B=@('ID_SUSPENSION_PITCH_UP');                      L='PIT+' }
              @{ N='Pitch Down';         U='susp_pit_dn';  T='Simple';    B=@('ID_SUSPENSION_PITCH_DOWN');                    L='PIT-' }
              @{ N='Roll Right';         U='susp_rol_up';  T='Simple';    B=@('ID_SUSPENSION_ROLL_UP');                       L='ROL>' }
              @{ N='Roll Left';          U='susp_rol_dn';  T='Simple';    B=@('ID_SUSPENSION_ROLL_DOWN');                     L='<ROL' }
              @{ N='Suspension Reset';   U='susp_reset';   T='Simple';    B=@('ID_SUSPENSION_RESET');                         L='RST' }
              @{ N='Steering Right';     U='steer_max';    T='Simple';    B=@('gm_steering_rangeMax');                        L='STR>' }
              @{ N='Steering Left';      U='steer_min';    T='Simple';    B=@('gm_steering_rangeMin');                        L='<STR' }
              @{ N='Throttle Max';       U='thr_max';      T='Simple';    B=@('gm_throttle_rangeMax');                        L='THR+' }
              @{ N='Throttle Min';       U='thr_min';      T='Simple';    B=@('gm_throttle_rangeMin');                        L='THR-' }
           )
        }

        @{ Id='tank.combat'; Asm='TankCombat'; Cat='WT - Tank Combat'; Accent=@(208,80,80);
           Actions=@(
              @{ N='Fire Secondary';     U='fire_secondary'; T='Simple';  B=@('ID_FIRE_GM_SECONDARY_GUN');                    L='FIRE2' }
              @{ N='Fire Special';       U='fire_special'; T='Simple';    B=@('ID_FIRE_GM_SPECIAL_GUN');                      L='SPEC' }
              @{ N='Select Primary';     U='sel_primary';  T='Simple';    B=@('ID_SELECT_GM_GUN_PRIMARY');                    L='G1' }
              @{ N='Select Secondary';   U='sel_secondary'; T='Simple';   B=@('ID_SELECT_GM_GUN_SECONDARY');                  L='G2' }
              @{ N='Select MG';          U='sel_mg';       T='Simple';    B=@('ID_SELECT_GM_GUN_MACHINEGUN');                 L='MG' }
              @{ N='Reset Selection';    U='sel_reset';    T='Simple';    B=@('ID_SELECT_GM_GUN_RESET');                      L='RST' }
              @{ N='Smoke Screen';       U='smoke';        T='Simple';    B=@('ID_SMOKE_SCREEN_GENERATOR');                   L='SMK' }
              @{ N='Repair';             U='repair';       T='Simple';    B=@('ID_REPAIR_TANK');                              L='RPR' }
              @{ N='Hull Aiming';        U='hull_aim';     T='Simple';    B=@('ID_ENABLE_GM_HULL_AIMING');                    L='HULL' }
              @{ N='Reload Guns';        U='reload';       T='Simple';    B=@('ID_RELOAD_GUNS');                              L='RELD' }
           )
        }

        @{ Id='tank.sights'; Asm='TankSights'; Cat='WT - Tank Sights'; Accent=@(160,224,80);
           Actions=@(
              @{ N='Rangefinder';        U='rangefinder';  T='Simple';    B=@('ID_RANGEFINDER');                              L='RNG' }
              @{ N='Targeting Hold';     U='targ_hold';    T='Simple';    B=@('ID_TARGETING_HOLD_GM');                        L='TARG' }
              @{ N='Zoom Hold';          U='zoom_hold';    T='Simple';    B=@('ID_ZOOM_HOLD_GM');                             L='ZMH' }
              @{ N='Zoom Toggle';        U='zoom_toggle';  T='Simple';    B=@('ID_ZOOM_TOGGLE');                              L='ZOOM' }
              @{ N='Crosshair Light';    U='xhair_light';  T='Simple';    B=@('ID_TOGGLE_GM_CROSSHAIR_LIGHTING');             L='XHRL' }
              @{ N='Tank NV';            U='tank_nv';      T='Simple';    B=@('ID_TANK_NIGHT_VISION');                        L='NV' }
              @{ N='Fuse Mode';          U='fuse_mode';    T='Simple';    B=@('ID_TANK_SWITCH_FUSE_MODE');                    L='FUSE' }
              @{ N='Thermal Polarity';   U='thermal';      T='Simple';    B=@('ID_THERMAL_WHITE_IS_HOT');                     L='THM' }
              @{ N='Sight Distance +';   U='sight_max';    T='Simple';    B=@('gm_sight_distance_rangeMax');                  L='SD+' }
              @{ N='Sight Distance -';   U='sight_min';    T='Simple';    B=@('gm_sight_distance_rangeMin');                  L='SD-' }
              @{ N='Sight Distance =';   U='sight_set';    T='Simple';    B=@('gm_sight_distance_rangeSet');                  L='SD=' }
           )
        }

        @{ Id='radar.ground'; Asm='RadarGround'; Cat='WT - Radar Ground'; Accent=@(136,176,64);
           Actions=@(
              @{ N='Tank Sensor Switch'; U='sensor_switch'; T='Simple';   B=@('ID_SENSOR_SWITCH_TANK');                       L='RDR' }
              @{ N='Tank Mode Switch';   U='mode_switch';  T='Simple';    B=@('ID_SENSOR_MODE_SWITCH_TANK');                  L='MODE' }
              @{ N='Tank Range Switch';  U='range_switch'; T='Simple';    B=@('ID_SENSOR_RANGE_SWITCH_TANK');                 L='RNG' }
              @{ N='Tank Scan Pattern';  U='scan_switch';  T='Simple';    B=@('ID_SENSOR_SCAN_PATTERN_SWITCH_TANK');          L='SCAN' }
              @{ N='Tank Target Lock';   U='target_lock';  T='Simple';    B=@('ID_SENSOR_TARGET_LOCK_TANK');                  L='LOCK' }
              @{ N='Tank Target Switch'; U='target_switch'; T='Simple';   B=@('ID_SENSOR_TARGET_SWITCH_TANK');                L='TGT>' }
              @{ N='Tank View Switch';   U='view_switch';  T='Simple';    B=@('ID_SENSOR_VIEW_SWITCH_TANK');                  L='VIEW' }
              @{ N='Weapon Lock Tank';   U='weapon_lock';  T='Simple';    B=@('ID_WEAPON_LOCK_TANK');                         L='WLCK' }
              @{ N='IRCM Switch';        U='ircm';         T='Simple';    B=@('ID_IRCM_SWITCH_TANK');                         L='IRCM' }
              @{ N='APS Lock';           U='aps_lock';     T='Simple';    B=@('ID_LOCK_TARGETING_AT_POINT_SHIP');             L='APS' }
           )
        }

        @{ Id='ship.combat'; Asm='ShipCombat'; Cat='WT - Ship Combat'; Accent=@(96,144,224);
           Actions=@(
              @{ N='Lock Target';        U='lock_pt';      T='Simple';    B=@('ID_LOCK_TARGETING_AT_POINT_SHIP');             L='LKPT' }
              @{ N='Ship Zoom Max';      U='zoom_max';     T='Simple';    B=@('ship_zoom_rangeMax');                          L='ZOOM+' }
           )
        }

        @{ Id='view'; Asm='View'; Cat='WT - View'; Accent=@(224,128,176);
           Actions=@(
              @{ N='Toggle View';        U='toggle';       T='Simple';    B=@('ID_TOGGLE_VIEW');                              L='VIEW' }
              @{ N='Heli View';          U='toggle_heli';  T='Simple';    B=@('ID_TOGGLE_VIEW_HELICOPTER');                   L='HVW' }
              @{ N='Camera Neutral';     U='neutral';      T='Simple';    B=@('ID_CAMERA_NEUTRAL');                           L='CTR' }
              @{ N='View Down';          U='view_down';    T='Simple';    B=@('ID_CAMERA_VIEW_DOWN');                         L='DOWN' }
              @{ N='Driver Camera';      U='driver';       T='Simple';    B=@('ID_CAMERA_DRIVER');                            L='DRV' }
              @{ N='Binoculars';         U='binoc';        T='Simple';    B=@('ID_CAMERA_BINOCULARS');                        L='BINO' }
              @{ N='Target Camera';      U='tgt_cam';      T='Simple';    B=@('ID_TARGET_CAMERA');                            L='TGT' }
              @{ N='UAV Camera';         U='uav';          T='Simple';    B=@('ID_TOGGLE_UAV_CAMERA');                        L='UAV' }
              @{ N='Cam X Center';       U='cam_x';        T='Simple';    B=@('camx_rangeSet');                               L='CAMX' }
              @{ N='Cam Y Center';       U='cam_y';        T='Simple';    B=@('camy_rangeSet');                               L='CAMY' }
           )
        }

        @{ Id='coms'; Asm='Coms'; Cat='WT - Comms'; Accent=@(80,112,208);
           Actions=@(
              @{ N='Voice Msg 2';        U='voice2';       T='Simple';    B=@('ID_VOICE_MESSAGE_2');                          L='V2' }
              @{ N='Voice Msg 7';        U='voice7';       T='Simple';    B=@('ID_VOICE_MESSAGE_7');                          L='V7' }
              @{ N='Push to Talk';       U='ptt';          T='Simple';    B=@('ID_PTT');                                      L='PTT' }
              @{ N='Squad Voice List';   U='voice_squad';  T='Simple';    B=@('ID_SHOW_VOICE_MESSAGE_LIST_SQUAD');            L='SQDV' }
              @{ N='Squad Designate';    U='squad_des';    T='Simple';    B=@('ID_SQUAD_TARGET_DESIGNATION');                 L='SQDD' }
              @{ N='Support Plane';      U='support';      T='Simple';    B=@('ID_START_SUPPORT_PLANE');                      L='SUPP' }
              @{ N='Plane Orbit';        U='orbit';        T='Simple';    B=@('ID_SUPPORT_PLANE_ORBITING');                   L='ORB' }
              @{ N='Wheel Menu';         U='wheel';        T='Simple';    B=@('ID_SHOW_MULTIFUNC_WHEEL_MENU');                L='WHEEL' }
           )
        }

        @{ Id='hud'; Asm='Hud'; Cat='WT - HUD & System'; Accent=@(128,128,144);
           Actions=@(
              @{ N='Hide HUD';           U='hide_hud';     T='Simple';    B=@('ID_HIDE_HUD');                                 L='HUD' }
              @{ N='Pause';              U='pause';        T='Simple';    B=@('ID_GAME_PAUSE');                               L='PAUS' }
              @{ N='Screenshot';         U='screenshot';   T='Simple';    B=@('ID_SCREENSHOT');                               L='SHOT' }
              @{ N='Flight Menu';        U='flightmenu';   T='Simple';    B=@('ID_FLIGHTMENU');                               L='MENU' }
              @{ N='Flight Setup';       U='flightsetup';  T='Simple';    B=@('ID_FLIGHTMENU_SETUP');                         L='STUP' }
              @{ N='MP Stat Screen';     U='mp_stat';      T='Simple';    B=@('ID_MPSTATSCREEN');                             L='STAT' }
              @{ N='Instructor';         U='instructor';   T='Simple';    B=@('ID_TOGGLE_INSTRUCTOR');                        L='INST' }
              @{ N='Action Bar 5';       U='ab5';          T='Simple';    B=@('ID_ACTION_BAR_ITEM_5');                        L='AB5' }
              @{ N='Action Bar 6';       U='ab6';          T='Simple';    B=@('ID_ACTION_BAR_ITEM_6');                        L='AB6' }
              @{ N='Action Bar 7';       U='ab7';          T='Simple';    B=@('ID_ACTION_BAR_ITEM_7');                        L='AB7' }
              @{ N='Action Bar 8';       U='ab8';          T='Simple';    B=@('ID_ACTION_BAR_ITEM_8');                        L='AB8' }
              @{ N='Action Bar 9';       U='ab9';          T='Simple';    B=@('ID_ACTION_BAR_ITEM_9');                        L='AB9' }
              @{ N='Continue';           U='continue';     T='Simple';    B=@('ID_CONTINUE');                                 L='CONT' }
              @{ N='Control Mode';       U='ctrl_mode';    T='Simple';    B=@('ID_CONTROL_MODE');                             L='CTRL' }
              @{ N='Shot Frequency';     U='shot_freq';    T='Simple';    B=@('ID_CHANGE_SHOT_FREQ');                         L='FREQ' }
              @{ N='Exit Cycle';         U='exit_cycle';   T='Simple';    B=@('ID_EXIT_SHOOTING_CYCLE_MODE');                 L='EXIT' }
           )
        }

    )
}
