
m(3).
n(4).

empty(0).
naught(1).
cross(2).

:- dynamic m/1.
:- dynamic n/1.

depth_limit(3).


win_score(Cross, -1) :- cross(Cross).
win_score(Naught, 1) :- naught(Naught).

min(X, Y, X) :- X < Y,!. 
min(_, Y, Y). 

max(X, Y, X) :- X > Y,!. 
max(_, Y, Y). 

mydiv(Dividend, Divisor, Quotient) :-
    Quotient is Dividend // Divisor.

nth0(0, [Head|_], Head) :- !.

nth0(N, [_|Tail], Elem) :-
    nonvar(N),
    M is N-1,
    nth0(M, Tail, Elem).

nth0(N,[_|T],Item) :-       
    var(N),         
    nth0(M,T,Item),
    N is M + 1.

nth0(0, [Head|Tail], Head, Tail) :- !.

nth0(N, [Head|Tail], Elem, [Head|Rest]) :-
    nonvar(N),
    M is N-1,
    nth0(M, Tail, Elem, Rest).

nth0(N, [Head|Tail], Elem, [Head|Rest]) :-  
    var(N),                
    nth0(M, Tail, Elem, Rest),     
    N is M+1.
    
grid_index(Index, I, J) :-
    n(N),
    (var(Index) ->
        Index is N*I + J
    ;
        mydiv(Index, N, I),
        J is Index mod N
    ).

replace_at(Index, List, NewElem, Result) :-
    nth0(Index, List, _, Rest),
    nth0(Index, Result, NewElem, Rest).

compare_tuples((X1, Y1), (X2, Y2)) :-
    (X1 > X2 ; (X1 = X2, Y1 > Y2)).

compare_tuples0((X1, _), (X2, _)) :- X1 > X2.

compare_tuples1((_, Y1), (_, Y2)) :- Y1 > Y2.

fmax_list([H|T], Comparator, Max) :-
    fmax_list(T, Comparator, H, Max), !.

fmax_list([], _, Max, Max).

fmax_list([H|T], Comparator, CurrentMax, Max) :-
    call(Comparator, H, CurrentMax),
    fmax_list(T, Comparator, H, Max).  

fmax_list([_|T], Comparator, CurrentMax, Max) :-
    fmax_list(T, Comparator, CurrentMax, Max). 

set_board_value(BoardValue, Num) :-
    ToRemove =.. [BoardValue, _],
    Fact =.. [BoardValue, Num],
    retractall(ToRemove),
    assertz(Fact). 

set_dimensions(M, N) :- 
    set_board_value(m, M), 
    set_board_value(n, N).

opponent(Player, Opponent) :- cross(Player), naught(Opponent).
opponent(Player, Opponent) :- naught(Player), cross(Opponent).

opposite_comparator(min, max).
opposite_comparator(max, min).

apply(PArgs, AdditionalArgs) :-
    append(PArgs, AdditionalArgs, FullPArgs),
    Call =.. FullPArgs,
    call(Call).

for_each(PArgs, List, AccIn, AccOut) :- 
    for_each_util(PArgs, List, 0, AccIn, AccOut).

for_each_util(_, [], _,  Acc, Acc).
for_each_util(PArgs, [Elem|Elems], Index, AccIn, AccOut) :-
    apply(PArgs, [Elem, Index, AccIn, AccNext]),
    NextIndex is Index + 1,
    for_each_util(PArgs, Elems, NextIndex, AccNext, AccOut),!.

fmap_list(_, [], []).
fmap_list(PArgs, [X|Xs], [Y|Ys]) :-
    apply(PArgs, [X, Y]),
    fmap_list(PArgs, Xs, Ys).

find_win_sequence(OrigBoard, NextIndexPArgs, IndTripletCheckPArgs, Elem, Index0, AccIn, AccNext) :- 
    (
        empty(Empty),
        Elem \= Empty,
        apply(NextIndexPArgs, [Index0, Index1]),
        apply(NextIndexPArgs, [Index1, Index2]),
        apply(IndTripletCheckPArgs, [Index0, Index1, Index2]),
        nth0(Index0, OrigBoard, Elem),
        nth0(Index1, OrigBoard, Elem),
        nth0(Index2, OrigBoard, Elem),
        AccNext = [[Index0, Index1, Index2] | AccIn]
    );
    AccNext = AccIn.

shift_rght(Index, NxtIndex) :- NxtIndex is 1 + Index.
shift_left(Index, NxtIndex) :- NxtIndex is Index - 1.
shift_down(Index, NxtIndex) :- n(N), NxtIndex is N + Index.
shift_up(Index, NxtIndex)   :- n(N), NxtIndex is Index - N.

shift_rght_down(Index, NxtIndex) :- shift_rght(Index, Rght), shift_down(Rght, NxtIndex).
shift_rght_up(Index, NxtIndex)   :- shift_rght(Index, Rght), shift_up(Rght, NxtIndex).
shift_left_down(Index, NxtIndex) :- shift_left(Index, Rght), shift_down(Rght, NxtIndex).
shift_left_up(Index, NxtIndex)   :- shift_left(Index, Rght), shift_up(Rght, NxtIndex).

within_one_row(Index0, Index1, Index2) :-
    grid_index(Index0, I, _),
    grid_index(Index1, I, _),
    grid_index(Index2, I, _).

within_one_col(Index0, Index1, Index2) :-
    grid_index(Index0, _, J),
    grid_index(Index1, _, J),
    grid_index(Index2, _, J).

within_one_dgn(Index0, Index1, Index2) :- 
    shift_rght_down(Index0, Index1),
    shift_rght_down(Index1, Index2),!.
within_one_dgn(Index0, Index1, Index2) :- 
    shift_rght_up(Index0, Index1),
    shift_rght_up(Index1, Index2),!.
within_one_dgn(Index0, Index1, Index2) :- 
    shift_left_down(Index0, Index1),
    shift_rght_up(Index0, Index2),!.
within_one_dgn(Index0, Index1, Index2) :- 
    shift_left_up(Index0, Index1),
    shift_rght_down(Index0, Index2),!.

partial_victory_sequences(Board, ShiftPArgs, CheckIndexTripletPArgs, Sequences) :-
    PArgs = [find_win_sequence, Board, ShiftPArgs, CheckIndexTripletPArgs],
    for_each(PArgs, Board, [], Sequences).

all_victory_rows(Board, Rows) :-
    partial_victory_sequences(Board, [shift_rght], [within_one_row], Rows).

all_victory_cols(Board, Cols) :-
    partial_victory_sequences(Board, [shift_down], [within_one_col], Cols).

all_victory_dgns(Board, Dgns) :-
    partial_victory_sequences(Board, [shift_rght_down], [within_one_dgn], Dgns).

all_victory_sequences(Board, Sequences) :-
    all_victory_rows(Board, Rows),
    all_victory_cols(Board, Cols),
    all_victory_dgns(Board, Dgns),
    append(Rows, Cols, RC),
    append(RC, Dgns, Sequences).

neighbourhood(I, [I, Y, Z]) :- shift_up(I, Y)  , shift_up(Y, Z)  .
neighbourhood(I, [I, Y, Z]) :- shift_rght(I, Y), shift_rght(Y, Z).
neighbourhood(I, [I, Y, Z]) :- shift_down(I, Y), shift_down(Y, Z).
neighbourhood(I, [I, Y, Z]) :- shift_left(I, Y), shift_left(Y, Z).

neighbourhood(I, [I, Y, Z]) :- shift_rght_up(I, Y), shift_rght_up(Y, Z).
neighbourhood(I, [I, Y, Z]) :- shift_left_up(I, Y), shift_left_up(Y, Z).
neighbourhood(I, [I, Y, Z]) :- shift_rght_down(I, Y), shift_rght_down(Y, Z).
neighbourhood(I, [I, Y, Z]) :- shift_left_down(I, Y), shift_left_down(Y, Z).

neighbourhood(I, [X, I, Z]) :- shift_up(I, X), shift_down(I, Z).
neighbourhood(I, [X, I, Z]) :- shift_left(I, X), shift_rght(I, Z).
neighbourhood(I, [X, I, Z]) :- shift_rght_up(I, X), shift_left_down(I, Z).
neighbourhood(I, [X, I, Z]) :- shift_left_up(I, X), shift_rght_down(I, Z).

leads_to_victory(NextBoard, ChoiceIndex, CurrPlayer) :- 
    neighbourhood(ChoiceIndex, [X, Y, Z]),
    (
        within_one_row(Z, Y, X);
        within_one_col(Z, Y, X);
        within_one_dgn(Z, Y, X)
    ),
    nth0(Z, NextBoard, CurrPlayer),
    nth0(Y, NextBoard, CurrPlayer),
    nth0(X, NextBoard, CurrPlayer),!.

max_tuple((X1, Y1), (X2, _), (X1, Y1)) :- X1 >= X2,!.
max_tuple((_, _), (X2, Y2), (X2, Y2)).

min_tuple((X1, Y1), (X2, _), (X1, Y1)) :- X1 =< X2,!.
min_tuple((_, _), (X2, Y2), (X2, Y2)).

opposite2(max_tuple, min_tuple).
opposite2(min_tuple, max_tuple).

unit(_, _, _, CurrPlayer, _, _, (Gain, Index), (Gain, Index)) :- win_score(CurrPlayer, Gain),!.
unit(Depth, Comparator, Board, CurrPlayer, Elem, Index, AccIn, AccNext) :- 
    empty(Elem),
    replace_at(Index, Board, CurrPlayer, NextBoard),
    (leads_to_victory(NextBoard, Index, CurrPlayer) ->
        win_score(CurrPlayer, Gain),
        AccNext = (Gain, Index),!
    ;
        opponent(CurrPlayer, NextPlayer),
        opposite2(Comparator, NextComparator),
        decide2(Depth, NextComparator, NextBoard, NextPlayer, NextTuple),
        % write((Comparator, AccIn, NextTuple, AccNext)),
        call(Comparator, AccIn, NextTuple, AccNext)
    ),!.
unit(_, _, _, _, _, _, X, X).

% decide2(Depth, min_tuple, _, _, (2, -1)) :- depth_limit(Limit), Depth > Limit,!.
decide2(Depth, _, _, _, (0, -1)) :- depth_limit(Limit), Depth > Limit,!.
decide2(Depth, Comparator, Board, CurrPlayer, Tuple) :- 
    NextDepth is Depth + 1,
    PArgs = [unit, NextDepth, Comparator, Board, CurrPlayer],
    for_each(PArgs, Board, (-2, -1), Tuple).

next(Board, CurrPlayer, Index) :- decide2(0, max_tuple, Board, CurrPlayer, (_, Index)).