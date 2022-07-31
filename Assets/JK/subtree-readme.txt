per aggiungere un subtree

1) aggiungi il remote a cui punterà il subtree
	git remote add -f <NOME REMOTE> <URL>

2) fai un primo "pull" del subtree (N.B. devi essere nella cartella root)
	git subtree add -P <CARTELLA> <NOME REMOTE> master --squash

3) quando vorrai pullare nuovi commit del subree (N.B. devi essere nella cartella root)
	git subtree pull -P <CARTELLA> <NOME REMOTE> master --squash
	(N.B. squash è importante, senza non va)
	(se da conflitto esegui anche:)
	git checkout --theirs .
	git commit -a

4) per pushare sul subtree (N.B. devi essere nella cartella root)
	git subtree push -P <CARTELLA> <NOME REMOTE> master
	(N.B. è super solido e funziona anche per commit in cui hai modificato sia file locali che file del subtree)
	(N.B. in ogni caso meglio fare un commit in cui l'unica cosa che hai fatto è modificare il subtree)