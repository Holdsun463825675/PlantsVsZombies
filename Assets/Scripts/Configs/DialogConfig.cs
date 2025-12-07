using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogConfig : MonoBehaviour
{
    public const string menu_new_user = "请输入新的用户名（最好英文）";
    public const string menu_new_user_success = "新用户创建成功！";
    public const string menu_new_user_failure_blank = "用户名不能为空！";
    public const string menu_new_user_failure_overflow = "用户名不能超过16个字符！";
    public const string menu_rename_user_success = "重命名用户成功！";
    public const string menu_rename_user_failure = "重命名用户失败！";
    public const string menu_delete_user = "删除选中的用户，确定吗？";
    public const string menu_cant_delete_user = "无法删除当前活跃的用户！";
    public const string menu_delete_user_success = "删除用户成功！";
    public const string menu_delete_user_failure = "删除用户失败！";
    

    public const string level_skipLevel = "跳过当前页的所有关卡并获得奖励，确定吗？";

    public const string game_restart = "重新开始这局游戏，确定吗？";
    public const string game_skipLevel = "跳过这关的同时获得奖励，确定吗？";
    public const string game_backtoHome = "返回主菜单，这局游戏进度不会被保存，确定吗？";
    
}
