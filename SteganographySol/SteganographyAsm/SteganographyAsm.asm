;	TEMAT PROJEKTU: STEGANOGRAFIA
;	OPIS: PROGRAM MA ZA ZADANIE ZAKODOWANIE W BITACH WGRYWANEGO OBRAZU WIADOMOŒCI
;	TEKSTOWEJ. JEST TO MO¯LIWE POPRZEZ ZMIANÊ NAJMNIEJ ZNACZ¥CEGO BITU.
;	KODOWANIE ODBYWA SIÊ ZA POMOC¥ FUNCJI NAPISANEJ W JÊZYKU WYSOKOPOZIOMOWYM
;	C#, B¥DZ PROCEDURY ASEMBLEROWEJ. DODATKOWO PROGRAM DEKODUJE ZASZYFROWANE WIADOMOŒCI
;	W ANALOGICZNY SPOSÓB DO WYBRANEGO ZAKODOWANIA.
;	DATA: 06/02/23
;	AUTOR: JOLANTA LACHMAN
 
.code
;	Procedura checkMMXCapability
;	Parametry wejœciowe: brak
;	Parametry wyjœciowe: 0/1 (przez RAX)
;	Sprawdza czy procesor obs³uguje rozszerzenie MMX.
;	Zwraca zmienn¹ bool. Zmieniany jest rejestr rax przez który przekazywana jest
;	zmienna wyjœciowa i rdx. Rbx przywracany jest ze stosu.

checkMMXCapability PROC 
	push rbx			; Zapisanie rbx na stosie
	pushfq				; Wypchniêcie flag
	pop rax				; Pobranie flag do rax
	mov rbx, rax		; Zapisanie rax (flag) do rbx
	xor rax, 200000h	; Prze³¹czenie id flagi
	push rax			; Wypchniêcie prze³¹czonych flag
	popfq				; Pobierz je z powrotem do flag -> zaczynamy sprawdzaæ, czy siê zresetowa³y
	pushfq				; Wypchniêcie flag
	pop rax				; Pobranie flag z powrotem do rax
	cmp rax, rbx		; Porównanie aktualnych flag z wczeœniej zapisanymi
	jz ThatsABity		; Jesli siê zresetowa³y = nie mo¿na u¿ywaæ CPUID

	pop rbx				; Przywrócenie rbx
	mov eax, 1			; Nie mo¿na u¿ywaæ CPUID, wiêc..
	cpuid				; CPUID_0000_0001
	shr rdx, 23			; Przesuniêcie bitu MMX do pozycji najbardziej po prawej
	and rdx, 1			; Wykonuje AND na rdx (jesli bylo 1, pozostanie 1)
	mov rax, rdx		; Przeniesienie rdx do rax
	ret					; Zwrócenie 1 lub 0

ThatsABity:
	pop rbx				; Procesor nie obsluguje CPUID lub MMX
	xor rax, rax		; Resetowanie rax
	ret
CheckMMXCapability ENDP


;	Procedura EncryptAsm
;	Parametry wejœciowe:  wskaŸnik na tablice(byte* przez RCX), wskaŸnik na znak(byte* przez RDX)
;	Parametry wyjœciowe: brak
;	Zakodowywuje znak w bitmapie. Przyjmuje w rcx wskaznik na bitmape od momentu,
;	w ktorym ma zostac odczytana, a w rdx symbol, znak z tekstu do zakodowania w zdjeciu.
;	Nic nie zwraca, tylko modyfikuje argumenty. Rejestry s¹ wypchniête na stos, a póŸniej
;	przywrócone.

EncryptAsm proc 
	push rax			;zawartosc rejestrow idzie na stos
	push rbx
	push r8
	
	xor RAX, RAX		;zerowanie rax (Schowek na rozbity znak)
	xor RBX, RBX		;zerowanie rbx (Schowek na wskaznik na bitmape)
	mov RBX, RCX		;przeniesienie rcx do rbx
	xor RCX, RCX		;zerowanie rcx (Schowek na licznik petli)
	xor R8, R8

LetterLoop:
	shl RAX, 8			;przesuniecie wartosci bitu znaku na kolejny bajt
	shl R8, 8			;przesuniecie wartosci bitow do resetu na kolejny bajt
	mov AL, [RDX]		;wprowadzenie bitow znaku na najmniej znaczacy bajt RAX
	mov R8b, 11111110b	;wprowadzenie bitow resetu

	shl AL, CL;			;przesuniecie bitowe w lewo o wartosc iteratora petli
	shr AL, 7;			;powrotne przesuniecie bitu na skrajny punkt po prawej stronie
	inc CL				;inkrementacja CL

	cmp CL, 8			;zakonczenie petli gdy wykonana 8 powtorzen
	jne LetterLoop		;skok na poczatek petli

	movq mm0, qword ptr[RBX]		;pobranie osmiu bajtow zdjecia do mm0
	movq mm1, RAX					;pobranie rozbitych bitow tekstu do mm1
	movq mm2, R8					;pobranie rejestru resetu bitow do mm2

	pand mm0, mm2					;przypisanie bitow tekstu do najmniej znaczacych w bitmapie
	por mm0, mm1					;wykonanie operacji logicznej OR na obu rejestrach
	movq qword ptr[RBX], mm0		;zmiana danych w bitmapie
	emms							;czyszczenie mmx po wykorzystaniu

	pop r8							;przywrocenie wartosci ze stosu do rejestrow
	pop rbx						
	pop rax
    ret 

EncryptAsm endp

;	Procedura DecryptAsm
;	Parametry wejœciowe:  wskaŸnik na tablice(byte* przez RCX), wskaŸnik na znak(byte* przez RDX)
;	Parametry wyjœciowe:  odczytany znak(char przez RAX)
;	Odkodowywuje znak z bitmapy. Przyjmuje w rcx wskaznik na bitmape od momentu,
;	w ktorym ma zostac odczytana. Zwraca odkodowan¹ zmienn¹ znakow¹.
;	Rejestry (oprócz rax) s¹ wypchniête na stos, a póŸniej przywrócone.

DecryptAsm PROC C
	push rax			;zawartosc rejestrow idzie na stos
	push rbx
	
	xor RAX, RAX		;czyszczenie rax (Schowek na zawartosc linii resetu)
	xor RBX, RBX		;czyszczenie rbx (Schowek na wskaznik na bitmape)
	mov RBX, RCX		;przepisanie zawartosci rcx do rbx
	xor RCX, RCX		;czyszczenie rcx (Schowek na licznik petli)
	movq mm0, qword ptr[RBX]	;pobranie zawartosci bit mapy
	xor RBX, RBX		;czyszczenie rbx (Schowek na wskaznik na bitmape)	

	mov AL, 00000001b	;wprowadzenie bitow resetu
	movq mm1, RAX		;pobranie linii resetu do mm1
	punpckldq mm1, mm1	;rozbicie bajtow na wszystkie wartosci rejestru
	packssdw mm1, mm1	;tzw (broadcast)
	packuswb mm1,mm1

	pand mm0, mm1		;pozostawienie jedynie danych tekstu
	movq RBX, mm0		;pobranie bitow z mm0 do rbx
	emms				;czyszczenie mmx po wykorzystaniu
	xor RAX, RAX		;czyszczenie rax

TextLoop:
	inc CL				;zwiekszenie licznika
	or AL, BL			;przypisanie wartosci bitu na odpowiednie miejsce
	shr RBX, 8			;przesuniecie wektora o 8 w prawo
	shl BL, CL			;przesuniecie bitu o iterator w lewo
	cmp CL, 8			;warunek zakonczenia petli
	jne TextLoop	

	pop rbx				;przywrocenie wartosci ze stosu do rejestrow
    ret 
DecryptAsm ENDP
end
