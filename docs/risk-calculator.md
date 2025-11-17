# Risk Calculator

## Raw Risk

The `Raw Risk` is calculated as:

1. Determine the `exposurePoint` based on where the values of `severity` (Catastrophic,Critical,Serious,Marginal,Negligible) and `frequency` (Frequent,Regular,Occasional,Uncommon,Rare) intersect. 

Example: `severity=Catastrophic` and `frequency=Frequent` is an exposure point of 25. The complete scale is:

```
Severity     | Frequency   | Exposure Point
-------------|-------------|----------------
Catastrophic | Frequent    | 25
Catastrophic | Regular     | 24
Critical     | Frequent    | 23
Critical     | Regular     | 22
Catastrophic | Occasional  | 21
Serious      | Frequent    | 20
Critical     | Occasional  | 19
Serious      | Regular     | 18
Catastrophic | Uncommon    | 17
Marginal     | Frequent    | 16
Serious      | Occasional  | 15
Critical     | Uncommon    | 14
Marginal     | Regular     | 13
Catastrophic | Rare        | 12
Negligible   | Frequent    | 11
Serious      | Uncommon    | 10
Marginal     | Occasional  |  9
Critical     | Rare        |  8
Negligible   | Regular     |  7
Marginal     | Uncommon    |  6
Serious      | Rare        |  5
Negligible   | Occasional  |  4
Negligible   | Uncommon    |  3
Marginal     | Rare        |  2
Negligible   | Rare        |  1
```

2. Finally determine `Raw Risk` based on where `exposurePoint` value and the values of `exposure` (Extensive,Widespread,Significant,Restricted,Negligible) intersect

Example: `exposurePoint=25` and `exposure=Negligible` is a raw risk of 111. The complete scale is:

```
Exposure Point | Exposure     | Raw Risk
---------------|--------------|-----------
25             |  Extensive   | 125
24             |  Extensive   | 124
25             |  Widespread  | 123
23             |  Extensive   | 122
24             |  Widespread  | 121
25             |  Significant | 120
22             |  Extensive   | 119
23             |  Widespread  | 118
24             |  Significant | 117
25             |  Restricted  | 116
21             |  Extensive   | 115
22             |  Widespread  | 114
23             |  Significant | 113
24             |  Restricted  | 112
25             |  Negligible  | 111
20             |  Extensive   | 110
21             |  Widespread  | 109
22             |  Significant | 108
23             |  Restricted  | 107
24             |  Negligible  | 106
19             |  Extensive   | 105
20             |  Widespread  | 104
21             |  Significant | 103
22             |  Restricted  | 102
23             |  Negligible  | 101
18             |  Extensive   | 100
19             |  Widespread  |  99
20             |  Significant |  98
21             |  Restricted  |  97
22             |  Negligible  |  96
17             |  Extensive   |  95
18             |  Widespread  |  94
19             |  Significant |  93
20             |  Restricted  |  92
21             |  Negligible  |  91
16             |  Extensive   |  90
17             |  Widespread  |  89
18             |  Significant |  88
19             |  Restricted  |  87
20             |  Negligible  |  86
15             |  Extensive   |  85
16             |  Widespread  |  84
17             |  Significant |  83
18             |  Restricted  |  82
19             |  Negligible  |  81
14             |  Extensive   |  80
15             |  Widespread  |  79
16             |  Significant |  78
17             |  Restricted  |  77
18             |  Negligible  |  76
13             |  Extensive   |  75
14             |  Widespread  |  74
15             |  Significant |  73
16             |  Restricted  |  72
17             |  Negligible  |  71
12             |  Extensive   |  70
13             |  Widespread  |  69
14             |  Significant |  68
15             |  Restricted  |  67
16             |  Negligible  |  66
11             |  Extensive   |  65
12             |  Widespread  |  64
13             |  Significant |  63
14             |  Restricted  |  62
15             |  Negligible  |  61
10             |  Extensive   |  60
11             |  Widespread  |  59
12             |  Significant |  58
13             |  Restricted  |  57
14             |  Negligible  |  56
 9             |  Extensive   |  55
10             |  Widespread  |  54
11             |  Significant |  53
12             |  Restricted  |  52
13             |  Negligible  |  51
 8             |  Extensive   |  50
 9             |  Widespread  |  49
10             |  Significant |  48
11             |  Restricted  |  47
12             |  Negligible  |  46
 7             |  Extensive   |  45
 8             |  Widespread  |  44
 9             |  Significant |  43
10             |  Restricted  |  42
11             |  Negligible  |  41
 6             |  Extensive   |  40
 7             |  Widespread  |  39
 8             |  Significant |  38
 9             |  Restricted  |  37
10             |  Negligible  |  36
 5             |  Extensive   |  35
 6             |  Widespread  |  34
 7             |  Significant |  33
 8             |  Restricted  |  32
 9             |  Negligible  |  31
 4             |  Extensive   |  30
 5             |  Widespread  |  29
 6             |  Significant |  28
 7             |  Restricted  |  27
 8             |  Negligible  |  26
 3             |  Extensive   |  25
 4             |  Widespread  |  24
 5             |  Significant |  23
 6             |  Restricted  |  22
 7             |  Negligible  |  21
 2             |  Extensive   |  20
 3             |  Widespread  |  19
 4             |  Significant |  18
 5             |  Restricted  |  17
 6             |  Negligible  |  16
 1             |  Extensive   |  15
 2             |  Widespread  |  14
 3             |  Significant |  13
 4             |  Restricted  |  12
 5             |  Negligible  |  11
 1             |  Widespread  |  10
 2             |  Significant |   9
 3             |  Restricted  |   8
 4             |  Negligible  |   7
 1             |  Significant |   6
 2             |  Restricted  |   5
 3             |  Negligible  |   4
 1             |  Restricted  |   3
 2             |  Negligible  |   2
 1             |  Negligible  |   1
```