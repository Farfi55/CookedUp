conf_Strict_Level(2).   

conf_ExtraStrict :- conf_Strict_Level(Level), Level >= 2.
conf_Strict :- conf_Strict_Level(Level), Level >= 1.


conf_gi_MustHave_WorkCounter(true).

conf_gi_MustHave_WorkCounter :- conf_gi_MustHave_WorkCounter(true).
-conf_gi_MustHave_WorkCounter :- conf_gi_MustHave_WorkCounter(false).

