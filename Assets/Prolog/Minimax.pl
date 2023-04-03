% % Define the dynamic predicate person/1
% :- dynamic person/1.

% % Define the existing database of people
% person(dave).
% person(mark).
% person(holly).

% % Define the add_person predicate to add a new person
% add_person(Name) :-
%     % Check if the person already exists
%     (person(Name) ->
%         write(Name), write(' is already in the database');
%     % If not, add the person to the database
%         assertz(person(Name)),
%         write(Name), write(' has been added to the database')),
%     nl. % Print a newline for formatting purposes


%next_board(X, 123413).


%%%%%%%%%%%%%%%%%

% % Define a predicate `victory_row(Board, M, Player)` that checks if there are three cells in a row on the Board with the same value Player (1 or -1).
% victory_row1(Board, M, N, Row) :-
%     Start is 0,
%     End is M * N,
%     findall(Cell, (nth0(Index, Board, Cell), between(Start, End, Index)), Row).
%     %length(Row, N),
%     %maplist(=(Player), Row).

% % Define a predicate `victory_col(Board, N, Player)` that checks if there are three cells in a column on the Board with the same value Player (1 or -1).
% victory_col1(Board, N, Player) :-
%     findall(Cell, (nth0(Index, Board, Cell), Index mod N =:= N - 1), Col),
%     length(Col, N),
%     maplist(=(Player), Col).

% % Define a predicate `victory_diag(Board, Player)` that checks if there are three cells in a diagonal on the Board with the same value Player (1 or -1).
% victory_diag1(Board, N, Player) :-
%     findall(Cell, (nth0(Index, Board, Cell), Index mod (N+1) =:= 0), Diag1),
%     length(Diag1, L),
%     findall(Cell, (nth0(Index, Board, Cell), Index mod (N-1) =:= N-1), Diag2),
%     length(Diag2, L),
%     maplist(=(Player), Diag1),
%     maplist(=(Player), Diag2).

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

find_win_sequence(_, [], _, _, _, _).
find_win_sequence(Board, [Empty | Cells], Index, Nxt, K, Seq) :- 
    empty(Empty), 
    NxtIndex is Index + 1,
    find_win_sequence(Board, Cells, NxtIndex, Nxt, K, Seq).
find_win_sequence(Board, [Cell | _], Index, Nxt, K, Seq) :-
    find_win_seq_util(Board, Index, Nxt, K, Cell, [Index], [], Seq).

find_win_seq_util(_, _, _, 0, _, CurrSeq, SeqAcum, Seq) :- append(SeqAcum, [CurrSeq], Seq).
find_win_seq_util(Board, StartIndex, Nxt, K, Player, CurrSeq, SeqAcum, Seq) :-
    call(Nxt, StartIndex, NxtIndex),
    nth0(NxtIndex, Board, Player),
    NxtK is K - 1,
    append(CurrSeq, [NxtIndex], NewCurrSeq),
    find_win_seq_util(Board, NxtIndex, Nxt, NxtK, Player, NewCurrSeq, SeqAcum, Seq).

shift_rght(Index, NxtIndex) :- NxtIndex is 1 + Index.
shift_down(Index, NxtIndex) :- n(N), NxtIndex is N + Index.
shift_diag(Index, NxtIndex) :- n(N), NxtIndex is 1 + N + Index.

all_sequences(Board, Nxt, K, Seq) :-
    ReducedK is K - 1,
    find_win_sequence(Board, Board, 0, Nxt, ReducedK, Seq).

all_victory_rows(Board, Rows) :-
    all_sequences(Board, shift_rght, 3, Rows).

all_victory_cols(Board, Cols) :-
    all_sequences(Board, shift_down, 3, Cols).

all_victory_dgns(Board, Dgns) :-
    all_sequences(Board, shift_diag, 3, Dgns).

% victory_row (Board, M, N, Winner).
% victory_col (Board, M, N, Winner).
% victory_diag(Board, M, N, Winner).
victory_row(_, 2) :- false.
victory_col(_, 2) :- false.
victory_dgn(_, 2) :- false.

winner(Board, Winner) :- 
    victory_row(Board, Winner); 
    victory_col(Board, Winner); 
    victory_dgn(Board, Winner).

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
