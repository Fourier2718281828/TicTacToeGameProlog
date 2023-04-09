:- dynamic cross/1.
:- dynamic naught/1.
:- dynamic empty/1.
:- dynamic m/1.
:- dynamic n/1.

setup() :- set_board_value(cross, 1), set_board_value(naught, -1), set_board_value(empty, 0).

set_board_value(BoardValue, Num) :-
    ToRemove =.. [BoardValue, _],
    Fact =.. [BoardValue, Num],
    retractall(ToRemove),
    assertz(Fact). 

set_dimensions(M, N) :- 
    set_board_value(m, M), 
    set_board_value(n, N).

grid_index(Index, I, J) :-
    n(N),
    (var(Index) ->
        Index is N*I + J
    ;
        I is Index div N,
        J is Index mod N
    ).

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
shift_down(Index, NxtIndex) :- n(N), NxtIndex is N + Index.
shift_diag(Index, NxtIndex) :- n(N), NxtIndex is 1 + N + Index.

within_one_row(Index0, Index1, Index2) :-
    grid_index(Index0, I, _),
    grid_index(Index1, I, _),
    grid_index(Index2, I, _).

within_one_col(Index0, Index1, Index2) :-
    grid_index(Index0, _, J),
    grid_index(Index1, _, J),
    grid_index(Index2, _, J).

within_one_dgn(_, _, _) :- true.

all_victory_sequences(Board, ShiftPArgs, CheckIndexTripletPArgs, Sequences) :-
    PArgs = [find_win_sequence, Board, ShiftPArgs, CheckIndexTripletPArgs],
    for_each(PArgs, Board, [], Sequences).

all_victory_rows(Board, Rows) :-
    all_victory_sequences(Board, [shift_rght], [within_one_row], Rows).

all_victory_cols(Board, Cols) :-
    all_victory_sequences(Board, [shift_down], [within_one_col], Cols).

all_victory_dgns(Board, Dgns) :-
    all_victory_sequences(Board, [shift_diag], [within_one_dgn], Dgns).

% victory_row (Board, M, N, Winner).
% victory_col (Board, M, N, Winner).
% victory_diag(Board, M, N, Winner).
victory_row(_, 2) :- false.
victory_col(_, 1) :- true.
victory_dgn(_, 3) :- true.

winner(Board, Winner) :- 
    victory_row(Board, Winner),!; 
    victory_col(Board, Winner),!; 
    victory_dgn(Board, Winner),!.

victory(Board, Winner) :-
    cross(Cross),
    naught(Naught),
    empty(Empty),
    (   winner(Board, Cross) -> Winner = Cross
    ;   winner(Board, Naught) -> Winner = Naught
    ;   Winner = Empty
    ).




    % victory_row(Board, M, N, 1) ;
    % victory_row(Board, M, N, -1) ;
    % victory_col(Board, N, 1) ;
    % victory_col(Board, N, -1) ;
    % victory_diag(Board, N, 1) ;
    % victory_diag(Board, N, -1).

board0([1, 1, 1, 0]).
board1([1, 1, 1, 0,   0, 0, 0, 0,     0, 0, 0, 0,    0, 0, 0, 0]).
board2([1, -1, 1, -1,   0, 1, 0, 0,     -1, -1, 1, 0,    -1, -1, 0, 0]).

%
%
% X 0 X 0
% _ X _ _
% 0 0 X _
% 0 0 _ _
%
%
% [1, -1, 1, -1,   0, 1, 0, 0,     -1, -1, 1, 0,    -1, -1, 0, 0]
%
%
