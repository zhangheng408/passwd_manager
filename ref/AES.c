AES加密算法源代码 
//AES.h

#define decrypt TRUE
#define encrypt FALSE
#define TYPE BOOL

typedef struct _AES{
int Nb;
int Nr;
int Nk;
unsigned long *Word;
unsigned long *State;
}AES;

/*
加密数据
byte *input 明文
byte *inSize 明文长
byte *out 密文存放的地方
byte *key 密钥key
byte *keySize 密钥长
*/
void Cipher(
unsigned char* input, 
int inSize, 
unsigned char* out, 
unsigned char* key, 
int keySize);

/*
解密数据
byte *input 密文
int *inSize 密文长
byte *out 明文存放的地方
byte *key 密钥key
int *keySize 密钥长
*/
void InvCipher(
unsigned char* input, 
int inSize, 
unsigned char* out, 
unsigned char* key, 
int keySize);

/*
生成加密用的参数AES结构
int inSize 块大小
byte* 密钥
int 密钥长
unsigned long 属性(标实类型)
返回AES结构指针
*/
AES *InitAES(AES *aes,
int inSize, 
unsigned char* key, 
int keySize, TYPE type);

/*
生成加密用的参数AES结构
int inSize 块大小
byte* 密钥
int 密钥长
返回AES结构指针
*/
AES *InitAES(
int inSize, 
unsigned char* key, 
int keySize, BOOL );

/*
加密时进行Nr轮运算
AES * aes 运行时参数
*/
void CipherLoop(
AES *aes);
/*
解密时进行Nr轮逆运算
AES * aes 运行时参数
*/
void InvCipherLoop(
AES *aes);

/*
释放AES结构和State和密钥库word
*/
void freeAES(
AES *aes);

//AES.cpp

#include "stdafx.h"
#include 
#include 
#include "AES.h"
unsigned char* SubWord(unsigned char* word);
unsigned long* keyExpansion(unsigned char* key, int Nk, int Nr,int);
/*
加密数据
byte *input 明文
byte *inSize 明文长
byte *out 密文存放的地方
byte *key 密钥key
byte *keySize 密钥长
*/
void Cipher(unsigned char* input, int inSize, unsigned char* out, unsigned char* key, int keySize)
{
AES aes ;
InitAES(&aes,inSize,key,keySize,encrypt);

memcpy(aes.State,input,inSize);
CipherLoop(&aes);
memcpy(out,aes.State,inSize);

}

/*
解密数据
byte *input 密文
int *inSize 密文长
byte *out 明文存放的地方
byte *key 密钥key
int *keySize 密钥长
*/
void InvCipher(unsigned char* input, int inSize, unsigned char* out, unsigned char* key, int keySize)
{
AES aes;
InitAES(&aes,inSize,key,keySize,decrypt);
memcpy(aes.State,input,inSize);
InvCipherLoop(&aes);
memcpy(aes.State,out,inSize);
}

/*
生成加密用的参数AES结构
int inSize 块大小
byte* 密钥
int 密钥长
返回AES结构指针
*/
AES *InitAES(AES *aes,int inSize, unsigned char *key, int keySize, TYPE type)
{
int Nb = inSize >>2,
Nk = keySize >>2,
Nr = Nb < Nk ? Nk:Nb+6;
aes->Nb = Nb;
aes->Nk = Nk;
aes->Nr = Nr;
aes->Word = keyExpansion(key,Nb,Nr,Nk);

aes->State = new unsigned long[Nb+3];
if(type)
aes->State += 3;
return aes;
}

/*
生成加密用的参数AES结构
int inSize 块大小
byte* 密钥
int 密钥长
返回AES结构指针
*/
AES *InitAES(int inSize, unsigned char* key, int keySize,unsigned long type)
{
return InitAES(new AES(),inSize,key,keySize,type);
}
/*
*/
void CipherLoop(AES *aes)
{
unsigned char temp[4];
unsigned long *word8 = aes->Word,
*State = aes->State;

int Nb = aes->Nb,
Nr = aes->Nr;

int r;
for (r = 0; r < Nb; ++r)
{
State[r] ^= word8[r];
}
for (int round =1; round {
word8 += Nb;
/*
假设Nb=4;
---------------------
| s0 | s1 | s2 | s3 |
---------------------
| s4 | s5 | s6 | s7 |
---------------------
| s8 | s9 | sa | sb |
---------------------
| sc | sd | se | sf |
---------------------
| | | | |
---------------------
| | | | |
---------------------
| | | | |
---------------------
*/
memcpy(State+Nb,State,12);
/*
Nb=4;
---------------------
| s0 | | | | 
---------------------
| s4 | s5 | | | 
--------------------- 
| s8 | s9 | sa | | 
---------------------
| sc | sd | se | sf | 
---------------------
| | s1 | s2 | s3 | 
--------------------- 
| | | s6 | s7 | 
---------------------
| | | | sb |
---------------------
*/
for(r =0; r {
/*
temp = {Sbox[s0],Sbox[s5],Sbox[sa],Sbox[sf]};
*/
temp[0] = Sbox[*((unsigned char*)State)];
temp[1] = Sbox[*((unsigned char*)(State+1)+1)];
temp[2] = Sbox[*((unsigned char*)(State+2)+2)];
temp[3] = Sbox[*((unsigned char*)(State+3)+3)];

*((unsigned char*)State) = Log_02[temp[0]] ^ Log_03[temp[1]] ^ temp[2] ^ temp[3];
*((unsigned char*)State+1) = Log_02[temp[1]] ^ Log_03[temp[2]] ^ temp[3] ^ temp[0];
*((unsigned char*)State+2) = Log_02[temp[2]] ^ Log_03[temp[3]] ^ temp[0] ^ temp[1];
*((unsigned char*)State+3) = Log_02[temp[3]] ^ Log_03[temp[0]] ^ temp[1] ^ temp[2];

*State ^= word8[r];
State++;
}
State -= Nb;
}

memcpy(State+Nb,State,12);

word8 += Nb;
for(r =0; r {
*((unsigned char*)State) = Sbox[*(unsigned char*)State];
*((unsigned char*)State+1) = Sbox[*((unsigned char*)(State+1)+1)];
*((unsigned char*)State+2) = Sbox[*((unsigned char*)(State+2)+2)];
*((unsigned char*)State+3) = Sbox[*((unsigned char*)(State+3)+3)];

*State ^= word8[r];
State++;
}
}
/*
解密时进行Nr轮逆运算
AES * aes 运行时参数
*/
void InvCipherLoop(AES *aes)
{
unsigned long *Word = aes->Word,
*State = aes->State;

int Nb = aes->Nb,
Nr = aes->Nr;

unsigned char temp[4];

int r =0;
Word += Nb*Nr;
for (r = 0; r < Nb; ++r)
{
State[r] ^= Word[r];
}

State -= 3;

for (int round = Nr-1; round > 0; --round)
{
/*
假设Nb=4;
--------------------- 
| | | | | 
--------------------- 
| | | | | 
--------------------- 
| | | | | 
--------------------- 
| s0 | s1 | s2 | s3 | 
--------------------- 
| s4 | s5 | s6 | s7 | 
--------------------- 
| s8 | s9 | sa | sb | 
--------------------- 
| sc | sd | se | sf | 
--------------------- 
*/
memcpy(State,State+Nb,12);
/*
Nb=4;
---------------------
| | | | s7 |
---------------------
| | | sa | sb |
--------------------- 
| | sd | se | sf |
---------------------
| s0 | s1 | s2 | s3 |
---------------------
| s4 | s5 | s6 | | 
--------------------- 
| s8 | s9 | | |
---------------------
| sc | | | |
---------------------
*/

Word -= Nb;
State += Nb+2;

for(r = Nb-1; r >= 0; r--)
{
/*
temp = {iSbox[s0],iSbox[sd],iSbox[sa],iSbox[s7]};
*/
temp[0] = iSbox[*(byte*)State];
temp[1] = iSbox[*((byte*)(State-1)+1)];
temp[2] = iSbox[*((byte*)(State-2)+2)];
temp[3] = iSbox[*((byte*)(State-3)+3)];

*(unsigned long*)temp ^= Word[r];

*(unsigned char*)State = Log_0e[temp[0]] ^ Log_0b[temp[1]] ^ Log_0d[temp[2]] ^ Log_09[temp[3]];
*((unsigned char*)State+1) = Log_0e[temp[1]] ^ Log_0b[temp[2]] ^ Log_0d[temp[3]] ^ Log_09[temp[0]];
*((unsigned char*)State+2) = Log_0e[temp[2]] ^ Log_0b[temp[3]] ^ Log_0d[temp[0]] ^ Log_09[temp[1]];
*((unsigned char*)State+3) = Log_0e[temp[3]] ^ Log_0b[temp[0]] ^ Log_0d[temp[1]] ^ Log_09[temp[2]];

State --;
}
State -= 2;
}

Word -= Nb;
memcpy(State,State+Nb,12);

State += Nb+2;
for(r = Nb-1; r >= 0; r--)
{

*(unsigned char*)State = iSbox[*(unsigned char*)State];
*((unsigned char*)State+1) = iSbox[*((unsigned char*)(State-1)+1)];
*((unsigned char*)State+2) = iSbox[*((unsigned char*)(State-2)+2)];
*((unsigned char*)State+3) = iSbox[*((unsigned char*)(State-3)+3)];

*State ^= Word[r];
State --;
}
}
/*
*--------------------------------------------
*|k0|k1|k2|k3|k4|k5|k6|k7|k8|k9|.......|Nk*4|
*--------------------------------------------
*Nr轮密钥库
*每个密钥列长度为Nb
*---------------------
*| k0 | k1 | k2 | k3 |
*---------------------
*| k4 | k5 | k6 | k7 |
*---------------------
*| k8 | k9 | ka | kb |
*---------------------
*| kc | kd | ke | kf |
*---------------------
*/
unsigned long* keyExpansion(byte* key, int Nb, int Nr, int Nk)
{
unsigned long *w =new unsigned long[Nb * (Nr+1)]; // 4 columns of bytes corresponds to a word

memcpy(w,key,Nk<<2);
unsigned long temp;

for (int c = Nk; c < Nb * (Nr+1); ++c)
{
//把上一轮的最后一行放入temp
temp = w[c-1];
//判断是不是每一轮密钥的第一行
if (c % Nk == 0) 
{
//左旋8位
temp = (temp<<8)|(temp>>24);
//查Sbox表
SubWord((byte*)&temp);
temp ^= Rcon[c/Nk];
}
else if ( Nk > 6 && (c % Nk == 4) ) 
{
SubWord((byte*)&temp);
}
//w[c-Nk] 为上一轮密钥的第一行
w[c] = w[c-Nk] ^ temp;
}
return w;
}

unsigned char* SubWord(unsigned char* word)
{
word[0] = Sbox[ word[0] ];
word[1] = Sbox[ word[1] ];
word[2] = Sbox[ word[2] ];
word[3] = Sbox[ word[3] ];
return word;
}
/*
释放AES结构和State和密钥库word
*/
void freeAES(AES *aes)
{
// for(int i=0;iNb;i++)
// {
// printf("%d\n",i);
// free(aes->State[i]);
// free(aes->Word[i]);
// }
// printf("sdffd");
}