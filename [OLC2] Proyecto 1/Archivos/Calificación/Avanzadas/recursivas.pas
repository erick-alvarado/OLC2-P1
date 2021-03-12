program Funciones;
function factorial(n: integer): integer;
begin
    if (n = 0) then
        factorial := 1
    else
        factorial := n * factorial(n - 1);
end;

function ackermann(m,n: integer): integer;
begin
    if (m = 0) then
        ackermann := n + 1
    else if (m>0) AND (n = 0) then
        ackermann := ackermann(m - 1, 1)
    else
        ackermann := ackermann(m - 1, ackermann(m,n - 1));
end;

procedure Hanoi(discos:integer; origen,aux,destino:string);
begin
    if(discos=1) then
        writeln('Mover Disco de ',origen,' a ',destino)
    else
        Begin
        Hanoi(discos-1,origen,destino,aux);
        writeln('Mover disco de ',origen,' a ',destino);
        Hanoi(discos-1,aux,origen,destino);
        End;
end;

begin
    writeln('1 Factorial');
    writeln(factorial(6));

    writeln('2 Ackermann');
    writeln(ackermann(3,4));
    
    writeln('3 Hanoi');
    Hanoi(3, 'A', 'B', 'C');
end.