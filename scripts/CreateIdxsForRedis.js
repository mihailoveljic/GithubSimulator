const Redis = require('ioredis');
const { exec } = require('child_process');

const redis = new Redis({
  host: 'localhost',
  port: 6379,
  password: 'eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81'
});

async function createIndexes() {
  try {
    await redis.send_command('FT.CREATE', 'idx:Branch', 'ON', 'HASH', 'PREFIX', '1', 'branch:', 'SCHEMA', 'Name', 'TEXT');

    await redis.send_command('FT.CREATE', 'idx:Comment', 'ON', 'HASH', 'PREFIX', '1', 'comment:', 'SCHEMA', 'Content', 'TEXT');

    await redis.send_command('FT.CREATE', 'idx:Issue', 'ON', 'HASH', 'PREFIX', '1', 'issue:', 'SCHEMA', 'Title', 'TEXT', 'Description', 'TEXT', 'AssigneeEmail', 'TEXT');

    await redis.send_command('FT.CREATE', 'idx:Label', 'ON', 'HASH', 'PREFIX', '1', 'label:', 'SCHEMA', 'Name', 'TEXT', 'Description', 'TEXT');

    await redis.send_command('FT.CREATE', 'idx:Milestone', 'ON', 'HASH', 'PREFIX', '1', 'milestone:', 'SCHEMA', 'Title', 'TEXT', 'Description', 'TEXT');

    await redis.send_command('FT.CREATE', 'idx:Repository', 'ON', 'HASH', 'PREFIX', '1', 'repository:', 'SCHEMA', 'Name', 'TEXT', 'Description', 'TEXT');

    await redis.send_command('FT.CREATE', 'idx:User', 'ON', 'HASH', 'PREFIX', '1', 'user:', 'SCHEMA', 'Name', 'TEXT', 'Surname', 'TEXT', 'Role', 'TEXT');

    console.log('Indexes created successfully');
  } catch (error) {
    console.error('Error creating indexes:', error);
    return;
  }
}

createIndexes().then(() => {
  console.log('Indexes created successfully. Now seeding data.');
  //seedAllData();
}).catch(error => {
  console.error('Failed to create indexes:', error);
});



//#################################################################################
// const Redis = require('ioredis');
// const redis = new Redis({
//   host: 'localhost',
//   port: 6379,
//   password: 'eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81'
// });

const repositories = [
  {
    _id: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    Name: 'REPOSITORY 1',
    Description: 'Description for Repository 1',
    Visibility: 0
  },
  {
    _id: '91494575-bff8-4c8e-8dac-8649059835ab',
    Name: 'REPOSITORY 2',
    Description: 'Description for Repository 2',
    Visibility: 1
  }
];
const milestones = [
  {
    _id: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    Title: 'MILESTONE 1',
    Description: 'Description for Milestone 1',
    DueDate: new Date('2023-12-01T12:00:00.000Z'),
    State: 0,
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e'
  },
  {
    _id: '91494575-bff8-4c8e-8dac-8649059835ab',
    Title: 'MILESTONE 2',
    Description: 'Description for Milestone 2',
    DueDate: new Date('2023-12-31T12:00:00.000Z'),
    State: 0,
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e'
  }
];
const issues = [
  {
    _id: 'f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a',
    Title: 'Issue 1',
    Description: 'Description for Issue 1',
    CreatedAt: new Date('2023-11-01T10:30:00.000Z'),
    Assigne: {
      Email: 'assignee1@example.com'
    },
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    MilestoneId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    TaskType: 1,
    Events: []
  },
  {
    _id: 'eafed591-3f43-43f8-b6c0-4ae0fb331cfb',
    Title: 'Issue 2',
    Description: 'Description for Issue 2',
    CreatedAt: new Date('2023-11-01T14:45:00.000Z'),
    Assigne: {
      Email: 'assignee2@example.com'
    },
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    MilestoneId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    TaskType: 1,
    Events: []
  },
  {
    _id: 'b285f7e4-8762-4d33-8391-08ac6a92d97b',
    Title: 'Issue 3',
    Description: 'Description for Issue 3',
    CreatedAt: new Date('2023-11-01T09:15:00.000Z'),
    Assigne: {
      Email: 'assignee3@example.com'
    },
    RepositoryId: '91494575-bff8-4c8e-8dac-8649059835ab',
    MilestoneId: '91494575-bff8-4c8e-8dac-8649059835ab',
    TaskType: 1,
    Events: []
  }
];
const branches = [
  {
    _id: '7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23',
    Name: 'feature-branch-1',
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    IssueId: 'f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a',
    Commits: [
      {
        OccuredAt: new Date('2023-12-01T12:00:00.000Z'),
        Description: 'Commit 1',
        Hash: '91494575-bff8-4c8e-8dac-8649059835ab'
      },
      {
        OccuredAt: new Date('2023-12-01T12:00:00.000Z'),
        Description: 'Commit 2',
        Hash: '81494575-bff8-4c8e-8dac-8649059835ab'
      }
    ]
  },
  {
    _id: 'b71a920d-dbbf-4a01-8b7d-b9b78413e9c4',
    Name: 'bugfix-branch-1',
    RepositoryId: '91494575-bff8-4c8e-8dac-8649059835ab',
    IssueId: 'eafed591-3f43-43f8-b6c0-4ae0fb331cfb'
  },
  {
    _id: '4a2b67b7-1049-40d8-9c84-fc2622ab8197',
    Name: 'develop',
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e'
  }
];
const pullRequests = [
  {
    _id: 'd1e3a5c6-2a9b-4a6f-97d3-1f38e639e688',
    Source: '7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23',
    Target: '4a2b67b7-1049-40d8-9c84-fc2622ab8197',
    IssueId: 'f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a',
    MilestoneId: 'c4f09d2a-0b3a-4dbd-9a26-8a6a982c8123',
    TaskType: 0,
    Events: []
  }
];
const comments = [
  {
    "_id": 'd1e3a5c6-2a9b-4a6f-97d3-1f38e639e688',
    "DateTimeOccured": new Date('2023-12-01T15:45:00.000Z'),
    "EventType": 2,
    "Content": "This is the content of the comment for Task 1",
    "TaskId": 'f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a'
  },
  {
    "_id": '7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23',
    "DateTimeOccured": new Date('2023-12-10T08:30:00.000Z'),
    "EventType": 2,
    "Content": "Comment content for Task 2",
    "TaskId": 'eafed591-3f43-43f8-b6c0-4ae0fb331cfb'
  },
  {
    "_id": 'b71a920d-dbbf-4a01-8b7d-b9b78413e9c4',
    "DateTimeOccured": new Date('2023-12-20T12:15:00.000Z'),
    "EventType": 2,
    "Content": "Another comment for Task 3",
    "TaskId": 'b285f7e4-8762-4d33-8391-08ac6a92d97b'
  }
];
const labels = [
  {
    "_id": '91494575-bff8-4c8e-8dac-8649059835ab',
    "DateTimeOccured": new Date('2023-12-01T15:45:00.000Z'),
    "EventType": 1,
    "Name": "Label 1",
    "Description": "Description for Label 1",
    "Color": "#FFFF00"
  },
  {
    "_id": '92594575-bcc8-4c8e-8dac-8649059835cd',
    "DateTimeOccured": new Date('2023-12-01T15:45:00.000Z'),
    "EventType": 1,
    "Name": "Label 2",
    "Description": "Description for Label 2",
    "Color": "#0000FF"
  }
];
const users = [
  {
    "_id": '91794575-bff8-3c8e-8dac-8649059835cd',
    "Name": "User 1",
    "Surname": "Surname 1",
    "Role": "Role 1",
    "Mail": {
      "Email": 'user1@example.com'
    },
    "AccountCredentials": {
      "UserName": 'username1',
      "PasswordHash": 'passwordhash1'
    }
  },
  {
    "_id": '23694575-bff7-3d8e-8dbc-8649059835ab',
    "Name": "User 2",
    "Surname": "Surname 2",
    "Role": "Role 2",
    "Mail": {
      "Email": 'user2@example.com'
    },
    "AccountCredentials": {
      "UserName": 'username2',
      "PasswordHash": 'passwordhash2'
    }
  }
]

async function seedRepositories() {
  for (const repo of repositories) {
    const repoKey = `repository:${repo._id.toString('hex')}`;
    await redis.hset(repoKey, {
      'Id': repo._id,
      'Name': repo.Name,
      'Description': repo.Description,
      'Visibility': repo.Visibility
    });
  }
}

async function seedMilestones() {
  for (const milestone of milestones) {
    const milestoneKey = `milestone:${milestone._id.toString('hex')}`;
    await redis.hset(milestoneKey, {
      'Id': milestone._id,
      'Title': milestone.Title,
      'Description': milestone.Description,
      'DueDate': milestone.DueDate,
      'State': milestone.State,
      'RepositoryId': milestone.RepositoryId
    });
  }
}

async function seedIssues() {
  for (const issue of issues) {
    const issueKey = `issue:${issue._id.toString('hex')}`;
    await redis.hset(issueKey, {
      'Id': issue._id,
      'Title': issue.Title,
      'Description': issue.Description,
      'CreatedAt': issue.CreatedAt,
      'AssigneeEmail': issue.Assigne.Email,
      'RepositoryId': issue.RepositoryId,
      'MilestoneId': issue.MilestoneId,
      'TaskType': issue.TaskType,
      'Events': issue.Events
    });
  }
}
async function seedBranches() {
  for (const branch of branches) {
    const branchKey = `branch:${branch._id.toString('hex')}`;
    await redis.hset(branchKey, {
      'Id': branch._id,
      'Name': branch.Name,
      'RepositoryId': branch.RepositoryId,
      'IssueId': branch.IssueId,
      "Commits": branch.Commits
    });
  }
}

async function seedPullRequests() {
  for (const pullRequest of pullRequests) {
    const pullRequestKey = `pullRequest:${pullRequest._id.toString('hex')}`;
    await redis.hset(pullRequestKey, {
      'Id': pullRequest._id,
      'Source': pullRequest.Source,
      'Target': pullRequest.Target,
      'IssueId': pullRequest.IssueId,
      'MilestoneId': pullRequest.MilestoneId,
      'TaskType': pullRequest.TaskType,
      'Events': pullRequest.Events
    });
  }
}

async function seedComments() {
  for (const comment of comments) {
    const commentKey = `comment:${comment._id.toString('hex')}`;
    await redis.hset(commentKey, {
      'Id': comment._id,
      'DateTimeOccured': comment.DateTimeOccured,
      'EventType': comment.EventType,
      'Content': comment.Content,
      'TaskId': comment.TaskId
    });
  }
}

async function seedLabels() {
  for (const label of labels) {
    const labelKey = `label:${label._id.toString('hex')}`;
    await redis.hset(labelKey, {
      'Id': label._id,
      'DateTimeOccured': label.DateTimeOccured,
      'EventType': label.EventType,
      'Name': label.Name,
      'Description': label.Description,
      'Color': label.Color
    });
  }
}

async function seedUsers() {
  for (const user of users) {
    const userKey = `user:${user._id.toString('hex')}`;
    await redis.hset(userKey, {
      'Id': user._id,
      'Name': user.Name,
      'Surname': user.Surname,
      'Role': user.Role,
      'MailEmail': user.Mail.Email,
      'AccountCredentialsUsername': user.AccountCredentials.UserName,
      'AccountCredentialsPasswordHash': user.AccountCredentials.PasswordHash
    });
  }
}

async function seedAllData() {
  try {
    await seedRepositories();
    await seedMilestones();
    await seedIssues();
    await seedBranches();
    await seedPullRequests();
    await seedComments();
    await seedLabels();
    await seedUsers();
    console.log('All data seeded in Redis successfully');
  } catch (error) {
    console.error('Error seeding data:', error);
  }
}