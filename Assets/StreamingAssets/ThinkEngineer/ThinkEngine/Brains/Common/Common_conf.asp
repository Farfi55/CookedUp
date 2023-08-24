conf_Strict_Level(2).   

conf_ExtraStrict :- conf_Strict_Level(Level), Level >= 2.
conf_Strict :- conf_Strict_Level(Level), Level >= 1.
