drop Database MiniSocial
use master
create database MiniSocial
use MiniSocial
-- 1. Bảng Users (Người dùng)
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    AvatarUrl VARCHAR(255) NULL,
    Bio NVARCHAR(255) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 2. Bảng Posts (Bài viết)
CREATE TABLE Posts (
    PostId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    ImageUrl VARCHAR(255) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- 3. Bảng Comments (Bình luận)
CREATE TABLE Comments (
    CommentId INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PostId) REFERENCES Posts(PostId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE NO ACTION
);

-- 4. Bảng Likes (Lượt thích)
CREATE TABLE Likes (
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (PostId, UserId),
    FOREIGN KEY (PostId) REFERENCES Posts(PostId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE NO ACTION
);

-- 5. Bảng Follows (Theo dõi)
CREATE TABLE Follows (
    FollowerId INT NOT NULL,
    FollowingId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (FollowerId, FollowingId),
    FOREIGN KEY (FollowerId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (FollowingId) REFERENCES Users(UserId) ON DELETE NO ACTION
);

