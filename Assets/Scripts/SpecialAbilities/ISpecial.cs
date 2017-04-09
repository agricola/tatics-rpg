﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectType { None, Melee, Line, Ranged }

// Character = Ally + Enemy
public enum TargetType { Ally, Enemy, Character, Tile }

public interface ISpecial
{
    SelectType SelectType { get; }
    TargetType TargetType { get; }
    void Perform(Character actor, GameObject target = null);
}

//　　　　　　　　　　　　　　　　　　　　　　__＿__
//　　　　　　　　　　　　　　　　　　　´ : : : : : : : : ｀丶、
//　　　　　 　 　 　 　 　 　 　 ／ : : : : : : : : : : : : : : : :＼
//　　　　　　　　　　　　　　／: : : : : : : : : : : : : : : : : : : : :ヽ
//　　　　　　　　　　　 _｢∨ : : : : : : : : : : /{∠Ζ＼: : : : : : :,
//　　 　 　 　 　 　 ／: :V.: : : : : : : : : .:∨　　　　＿＼ : : : : :,
//　　　　 　 　 　 /ノ⌒7: : : : :|.: : : : : .::|　　　´　＿,,ハ: : : :ハ.
//.　　　　　 　 　 | ＼__ : : : : 八 : : : : l八　　　 ｲf笊＾Yﾚ|: : : :j:＼
//　 　 　 　 　 　 | : :〈_|: : : : : ::{＼ : 八　＼　 　 乂_ツ　|/| :/| : : ＼
//　　　　 　 　 　 | : : 八|: : : ::八 　>､__＞　　　　　　:::::.: 丿Ⅵ: | : |ハ
//　　　　　　　 　 | : : : 八: : : : : ∨　イf笊^　　　、　　 　 　 }:|: | : |／￣￣＼
//　　　　　　　 　 | : : : : :∧ : : : 人 ﾍ{ 乂_ツ　　　 　 /　　 ∧:.ﾉ:_/::::::::::::::::::::::-_
//　　　　　　　 　 | : |: : :　│: : : : : ＞ 　 :.:::::　　 ｰ　　　　/　/]:::::::::::::::::::::::::::::::::-_
//　　　　　　　 　 | : | : :| 　|│ : : : : (⌒丶、　　 　 　 　 ｲ／ /|::〈:::::::::::::::::::::::::::::::-_
//　　　　　　　 　 | : | : :| 　|人j: : : : :个ー: ｀''￢冖Ｔi:｢＼|＞ ﾞ┴/::::: |::::::::::::::::::::::::::-_
//　　　　　　　 　 | : | : :| 　 　 | : : : : : : : : : :＞‐=ﾆ广＞｛] ⌒∨￢:八::::::::::::::::::::::::::::-_
//　　　　　　　　　∨|: 八　　　| : : : : : : : : 〃:::::::::::{_/　　j|　　/::::/==:::＼{:::::::::::::::::::::::-_
//　　　　　　　　 　 人{　　　　八|: : : : : : : {{:::::::::::: 〈＼　八__/::::/::::::::::::: ∧:::::::::::::::::::::::-_
//　　　　　　　　 　 　 　 　 ／ :八: : : : : : : ∨::::＼〈::::::く.│/::::/:::::::::::::::::: ∧::::::::::::::::::::::::-_
//　　　　　　　　　　　　　　　　　　＼ : 卜､: :∨:::::::: ＼ ::＼|:::/:::::::::::::::::::::::::∧::::::::::::::::::::::::-_
//　　　　　　 　 　 　 　 　 　 　 　 　 ＼|　　ｰﾍ/:::::::|:::::::::::_∨ :::::::::::::::::::::::::: ∧::::::::::::::::::::::::-_
//　　　　　　　　　　　　 　 　 　 　 　 　 ＼.　　 ＼:八:::::::::（入 :::::::::::::::::::::::::::: ∧::::::::::::::::::::::::-_
//　　　　　　　　　　　　　　　　　　　　　　　　　　　∨:＼::::: ＿＼::::::::::::::::::::::::_ノ⌒^::::::::::::::::::::-_
//　　　　　　　　　　　　　　　　　　　__　　__rァ^ア＾''ー--＜＼:::::::￣￣￣￣::::::::::::::::::::::＼:::::::::-_
//　　　　　　　　　　　　　　　　　　r｢ {＞┴┴'┴'^ｰ=ﾆ二..,,＼＼:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::丿
//　　　　　　　　　　　　　　 　 　 ｢|_y'　　　　　　　　　　　　　¨''￢ﾆ()()::::::::::::::::＿＿;;::二ﾆ=-く
//　　　　　　　　　　　　　　　　 ／ 7　　　　　　　　　　　　　　　　　 ｀`''＜｢＼:::::└───::::::::＼
//　　　　　　　　　　　　　　　 　 　 {　　　　　　　 ＼_　　　　　　　　　　　　∨＾￣＼::::::::::::::::::::::::::∧
//　　　　　　　　　　　　　　　 {　　　　　　　　　　　　｀丶､　 　 　 　 　 　 　 ∨ﾆﾆ| |＼:::::::::::::::::::::::::廴___
//　　　　　　　　　　 　 　 　 八　　　`　、　　　　　　 　 　 　 　 　 　 　 　 　 ∨ﾆ.| |ﾆ. ＼::::::::::::::::::::::::::::::
//　　　　　　　　　　　　　　　　　　　　　 ＼　　　　　　　　　　｀ヽ　　　　　　　 ＼ | |ﾆﾆﾆ ＼::::::::::::::::::::::::
//　　　　 　 　 　 　 　 　 　 　 　 ＼　　　　＼　　　　　　　　　/∧　　　　　　　　〈〈＼＼ ﾆﾆ>､＿＿,／
//　　　　　　　　　　　　　 　 　 　 　 ヽ.　　　　＼　　　　 　 ／//∧　　　 　 　 　 ∨/ ＼＼／／＼/＿_