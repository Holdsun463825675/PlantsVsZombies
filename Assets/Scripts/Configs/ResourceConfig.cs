using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceConfig
{
    public const string music_mainMenu = "Audio/Music/MainMenu";
    public const string music_selectCard = "Audio/Music/SelectCard";
    public const string music_day = "Audio/Music/Day";

    public const string sound_buttonandputdown_tap = "Audio/Sound/buttonandputdown/tap";
    public const string sound_buttonandputdown_tap2 = "Audio/Sound/buttonandputdown/tap2";

    public const string sound_collectitem_sun = "Audio/Sound/collectitem/sun";

    public const string sound_placeplant_selectcard = "Audio/Sound/placeplant/selectcard";
    public const string sound_placeplant_plant = "Audio/Sound/placeplant/plant";
    public const string sound_plantshoot_throw = "Audio/Sound/plantshoot/throw";

    public static readonly string[] sound_bullethit_splats = new string[]
    {
        "Audio/Sound/bullethit/splat",
        "Audio/Sound/bullethit/splat2",
        "Audio/Sound/bullethit/splat3",
    };

    public static readonly string[] sound_other_groans = new string[]
    {
        "Audio/Sound/other/groan",
        "Audio/Sound/other/groan2",
        "Audio/Sound/other/groan3",
        "Audio/Sound/other/groan4",
        "Audio/Sound/other/groan5",
        "Audio/Sound/other/groan6",
    
    };
    public static readonly string[] sound_zombieeat_chomps = new string[]
    {
        "Audio/Sound/zombieeat/chomp",
        "Audio/Sound/zombieeat/chomp2"
    };
    public const string sound_zombieeat_gulp = "Audio/Sound/zombieeat/gulp";

    public const string sound_zombiedie_limbsPop = "Audio/Sound/zombiedie/limbs_pop";
    public static readonly string[] sound_zombiedie_fallings = new string[]
    {
        "Audio/Sound/zombiedie/falling1",
        "Audio/Sound/zombieeat/falling2"
    };

    public const string sound_clickfail_buzzer = "Audio/Sound/clickfail/buzzer";

    public const string sound_textsound_awooga = "Audio/Sound/textsound/awooga";
    public const string sound_textsound_siren = "Audio/Sound/textsound/siren";
    public const string sound_textsound_readysetplant = "Audio/Sound/textsound/readysetplant";
    public const string sound_textsound_hugewave = "Audio/Sound/textsound/hugewave";
    public const string sound_textsound_finalwave = "Audio/Sound/textsound/finalwave";

    public const string sound_win_winmusic = "Audio/Sound/win/winmusic";
    public const string sound_lose_losemusic = "Audio/Sound/lose/losemusic";
    public const string sound_lose_scream = "Audio/Sound/lose/scream";

    public static readonly string[] image_win_awards = new string[]
    {
        "Images/Card/card_peashooter",
        "Images/Card/card_sunflower",
        "Images/Card/card_cherrybomb"
    };

}
