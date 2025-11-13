# Rubik's Cube Cipher

A novel encryption system that uses Rubik's Cube move sequences as the cryptographic key. Messages are scrambled using virtual 3D cube rotations, with automatic key generation and embedded metadata for seamless decryption.

## 🎯 Overview

The Rubik's Cube Cipher treats a message as the state of a virtual 54-position Rubik's Cube (6 faces × 9 squares). By applying a sequence of random cube rotations (U, R, L, F, D, B), the message gets scrambled in a reversible way. Each encryption generates a unique key, and the encrypted output includes embedded metadata for automatic decryption.

### Key Features

- **Random Key Generation**: Each encryption creates a unique 6-move sequence
- **Self-Contained Ciphertext**: Metadata embedded in Base64 encoding
- **Multiple Move Types**: Supports standard (U), prime (U'), and double (U2) moves
- **Deterministic Permutations**: Uses seeded random (seed=42) for consistent scrambling
- **Automatic Decryption**: No need to manually share keys

## 🏗️ Project Structure

```
RubiksCubeCipher/
├── RubiksCubeCipherApp/         # Console application
│   ├── Program.cs                # CLI interface with random key generation
│   ├── RubiksCubeCipher.cs       # Core cipher logic
│   └── cipher.txt                # Saved ciphertext output
└── README.md                     # This file
```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 or .NET 9.0 SDK

### Running the Console App

```bash
cd RubiksCubeCipherApp
dotnet run
```

**Example Session:**
```
Enter message: The quick brown fox jumps over the lazy dog

Key: L2 R' U' U L U2

Encrypted: _whfe__o qjc T k__p er _g_ohy saoldebxnoi__u zr_ _tmvu|NDM6TDIsRicsUicsUixVLFUy

Decrypted: The quick brown fox jumps over the lazy dog
```

## 🔐 How It Works

### Encryption Process

1. **Message Encoding**: Input message (up to 54 chars) is mapped to cube positions
2. **Random Key Generation**: Creates 6 random moves from {U, R, L, F, D, B} with optional ', or 2
3. **Permutation Application**: Cube moves scramble the character positions
4. **Metadata Embedding**: Message length + move sequence encoded in Base64
5. **Output Format**: `<scrambled_text>|<base64_metadata>`

### Decryption Process

1. **Metadata Extraction**: Split ciphertext at `|` and decode Base64
2. **Key Retrieval**: Extract the original move sequence from metadata
3. **Inverse Moves**: Apply moves in reverse order with inverse operations
4. **Message Recovery**: Extract original message with correct length

### Move Notation

- **U, R, L, F, D, B**: Standard clockwise rotations (Up, Right, Left, Front, Down, Back)
- **U', R', L', F', D', B'**: Counter-clockwise rotations (prime moves)
- **U2, R2, L2, F2, D2, B2**: 180° rotations (double moves)

**Example Keys**: 
- `L2 R' U' U L U2`
- `B F2 D' R L' U`
- `U R F L' D B2`

## 🔧 Technical Details

### Cipher Configuration

**Random Key Generation** (`Program.cs`):
```csharp
var allMoves = new[] { "U", "R", "L", "F", "D", "B" };
var rng = new Random();
var key = Enumerable.Range(0, 6)
    .Select(_ => allMoves[rng.Next(allMoves.Length)] + 
                 (rng.Next(3) == 0 ? "'" : rng.Next(3) == 1 ? "2" : ""))
    .ToList();
```

### Algorithm Details

- **Cube State**: 54-character array (6 faces × 9 positions each)
- **Permutation Seed**: Random seed `42` ensures deterministic scrambling
- **Move Inverse Calculation**: Precomputed for O(1) lookup
- **Metadata Format**: `{length}:{move1,move2,...}` → Base64
- **File Storage**: Ciphertext saved to `cipher.txt`

### Inverse Move Logic

```csharp
X   → X'   (clockwise → counter-clockwise)
X'  → X    (counter-clockwise → clockwise)
X2  → X'2  (double → inverse double)
X'2 → X2   (inverse double → double)
```

## 📊 Example Usage

### Encrypt and Decrypt

```bash
$ dotnet run
Enter message: hello world

Key: R L2 F' D U2 B

Encrypted: ___o_lel____ ________r_____hd____o_w________l_________|MTE6UixMMixGJyxELFUyLEI=

Decrypted: hello world
```

### Saved Ciphertext

The encrypted message is automatically saved to `cipher.txt`:
```
_whfe__o qjc T k__p er _g_ohy saoldebxnoi__u zr_ _tmvu|NDM6TDIsRicsUicsUixVLFUy
```

## 🛠️ Building from Source

```bash
dotnet build RubiksCubeCipherApp/RubiksCubeCipherApp.csproj
```

Or run directly:
```bash
cd RubiksCubeCipherApp
dotnet run
```

## 🧪 Testing

The cipher ensures perfect round-trip encryption:
```
Original Message → Encrypt (Random Key) → Decrypt → Original Message ✓
```

Test with various inputs:
- **Short messages**: `"abc"` (padded to 54 chars internally)
- **Long messages**: `"The quick brown fox jumps over the lazy dog"` (43 chars)
- **Maximum length**: 54 characters (fills entire cube)
- **Special characters**: `"Hello, World! @2025 #cipher"`

## 🎓 Educational Value

This project demonstrates:
- **Permutation-Based Cryptography**: Using mathematical transformations for encryption
- **Metadata Embedding**: Self-describing encrypted data structure
- **Random Key Generation**: Secure, unpredictable move sequences
- **Algorithm Reversibility**: Ensuring lossless encryption/decryption
- **Group Theory**: Rubik's Cube as a permutation group

## 📝 Implementation Notes

- **Maximum message length**: 54 characters (matches 54 cube positions)
- **Cipher strength**: Educational/demonstrative (not cryptographically secure)
- **Permutations**: Deterministic scrambling with seed=42 for reproducibility
- **Key uniqueness**: Each encryption generates a different random key
- **File persistence**: Ciphertext automatically saved to `cipher.txt`

## 🔒 Security Considerations

⚠️ **This cipher is for educational purposes only** and should not be used for securing sensitive data:

- Uses pseudo-random key generation (predictable with seed knowledge)
- Limited keyspace (6 moves from 18 possibilities)
- Deterministic permutations can be reverse-engineered
- No resistance to cryptanalysis attacks

For real-world encryption, use established algorithms like AES-256.

## 🤝 Contributing

Feel free to enhance the project:
- Implement cryptographically secure key generation
- Add support for longer messages (multiple cubes)
- Create a graphical 3D cube visualization
- Add command-line arguments for custom keys
- Implement key export/import functionality
- Add encryption strength analysis tools

## 🙏 Acknowledgments

- Rubik's Cube notation based on standard WCA (World Cube Association) guidelines
- Inspired by permutation group theory in cryptography
- Built with .NET for cross-platform compatibility

---

**Created with ❤️ using .NET and C#**

*An educational exploration of permutation-based encryption using Rubik's Cube mathematics*
