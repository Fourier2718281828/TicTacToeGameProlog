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
