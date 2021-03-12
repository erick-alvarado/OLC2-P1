program ejemplo;
procedure saludo_saludo2();
begin
    writeln('entro a saludo 2');
end;

function saludo_saludo3():integer;
begin
    writeln('entro a saludo 3');
    saludo_saludo3 := 10;
    saludo_saludo2();
end;

procedure saludo();
begin
    writeln('Estoy saludando');
    saludo_saludo3();
end;
    
begin
    saludo();
end.

{
    Salida
    Estoy saludando                                                                                             
    entro a saludo 3                                                                                            
    entro a saludo 2
}