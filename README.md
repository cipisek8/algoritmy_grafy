# Dokumentace

## 1. Úvod do grafů

### Definice grafu
Graf se skládá z vrcholů a hran, které spojují dva vrcholy.

- **Orientovaný graf** – hrany mají směr (např. z A do B).
- **Neorientovaný graf** – hrany nemají směr (spojení mezi A a B je obousměrné).
- **Ohodnocený graf** – hrany mají přiřazené váhy (např. vzdálenost, čas, náklady).

### Kostra grafu
Hrany, které spolu spojují všechny vrcholy bez vytvoření cyklů (jeden vrchol má víc jak 2 hrany).

### Matice sousednosti vs. seznam sousedů
- **Matice sousednosti**: 2D pole, kde `M[i][j]` udává, zda existuje hrana mezi vrcholy `i` a `j` (a případně její váhu).
- **Seznam sousedů**: každý vrchol má seznam vrcholů spojených hranami (a případně váhy hran).

### Reálné příklady využití grafů
- **Navigace**: trasy mezi městy (např. GPS).
- **Počítačové sítě**: přenos dat mezi uzly.
- **Plánování**: harmonogramy projektů, závislosti úloh.
- **Sociální sítě**: propojení mezi uživateli.

---

## 2. Problém hledání nejkratší cesty

### Co znamená „nejkratší cesta“
Hledání cesty mezi dvěma vrcholy v grafu tak, aby součet vah hran byl minimální.

### Negativní hrany a jejich vliv
- **Negativní hrana**: hrana s negativní váhou (např. sleva, zisk).
- Pokud graf obsahuje **záporný cyklus**, neexistuje nejkratší cesta (lze nekonečně zkracovat).
- Některé algoritmy (např. Dijkstra) nefungují správně s negativními hranami.

---

## 3. Přehled algoritmů

### a) Dijkstrův algoritmus
- **Princip**: Greedy přístup – v každém kroku se vybírá uzel s aktuálně nejkratší známou vzdáleností. Používá **prioritní frontu** (nejčastěji min-heap).
- **Omezení**: Nepracuje správně s negativními hranami.
- **Časová složitost**:
  - S prioritní frontou (např. binární haldou): **O((V + E) log V)**
  - S maticí sousednosti: **O(V²)**

### b)  Bellman-Fordův algoritmus
- **Princip**: Opakovaná **relaxace hran**, typicky až **(V−1)** krát, kde `V` je počet vrcholů.
- **Výhoda**: Funguje i s negativními hranami.
- **Detekce záporných cyklů**: Po poslední iteraci lze zjistit, zda existuje cyklus se zápornou délkou.
- **Časová složitost**: **O(V × E)**

### c) Floyd-Warshallův algoritmus
- **Princip**: Dynamické programování – postupně se zvažuje, zda lze vzdálenost mezi dvojicí vrcholů zlepšit přes mezivrchol `k`.
- **Vhodný pro**: Výpočet nejkratších cest mezi **všemi dvojicemi vrcholů**.
- **Časová složitost**: **O(V³)**

---

## 4. Závěr

### Shrnutí poznatků
- Grafy slouží k modelování vztahů mezi objekty.
- Neexistuje univerzální algoritmus pro všechny situace – výběr záleží na typu grafu a požadavcích (rychlost, negativní hrany, počet vrcholů).
- Porozumění datové reprezentaci grafu je klíčové pro správnou implementaci.
